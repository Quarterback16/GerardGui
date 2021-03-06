using Helpers;
using Helpers.Interfaces;
using Helpers.Models;
using RosterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Butler.Models
{
	public class MediaMailerJob : Job
	{
		private const string K_RecipientsKey = "MediaLogRecipients";

		private const string mediaXmlFileName = ".\\xml\\media-mail-list.xml";

		public int LogsMailed { get; set; }

		protected IMailMan MailMan { get; set; }

		protected IDetectLogFiles LogFileDetector { get; set; }

		protected LogMaster LogMaster { get; set; }

		public MediaMailerJob(
			IMailMan mailMan, IDetectLogFiles logFileDetector, IConfigReader configReader,
			IKeepTheTime timekeeper )
		{
			Name = "Media Mailer";
			Logger = NLog.LogManager.GetCurrentClassLogger();
			MailMan = mailMan;
			InstructMailMan( configReader );
			LogMaster = new LogMaster( mediaXmlFileName );
			LogFileDetector = logFileDetector;
			TimeKeeper = timekeeper;
		}

		private void InstructMailMan(
            IConfigReader configReader )
		{
			var recipients = configReader.GetSetting( K_RecipientsKey );
			if ( string.IsNullOrEmpty( recipients ) )
			{
				throw new ApplicationException( "Recipients Key is empty" );
			}
			MailMan.AddRecipients( recipients );
		}

		public override bool IsTimeTodo(
            out string whyNot )
		{
			whyNot = string.Empty;
			base.IsTimeTodo( out whyNot );

			//  Job is rigged to send all missed logs anyway so this just results in 2 a day
			//if ( string.IsNullOrEmpty( whyNot ) )
			//{
			//	var dow = ( int ) TimeKeeper.CurrentDateTime().Day;
			//	if ( dow % 2 != 0 )
			//		whyNot = "Only runs on even numbered days";
			//}

			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );

			return ( string.IsNullOrEmpty( whyNot ) );
		}

		public override string DoJob()
		{
			LogsMailed = 0;

			var lastDate = new DateTime( 1, 1, 1 );

			// load files meta data ur interested in
			var keys = new List<string>();
			foreach ( System.Collections.DictionaryEntry de in LogMaster.TheHt )
				keys.Add( de.Key.ToString() );

			if ( ThereAreNoFiles( keys ) )
			{
				Logger.Trace( "  There are no Media log files in {0}", mediaXmlFileName );
			}

			foreach ( string key in keys )
			{
				var logitem = ( LogItem ) LogMaster.TheHt[ key ];
				lastDate = MailLogFiles( logitem );
				if ( lastDate != new DateTime( 1, 1, 1 ) )
				{
					if ( LogsMailed > 0 )
					{
						logitem.MailDate = lastDate;
						LogMaster.PutItem( logitem );
						Logger.Info( "  Log date set to {0:d}", lastDate );
					}
				}
			}

			LogMaster.Dump2Xml();  //  write changes if any

			var finishedMessage = $"  {Name} job - done. {LogsMailed} logs mailed";
			Logger.Info( finishedMessage );
			return finishedMessage;
		}

		private static bool ThereAreNoFiles( List<string> keys ) => !keys.Any();

		private DateTime MailLogFiles( LogItem logitem )
		{
			var lastDate = new DateTime( 1, 1, 1 );
			var filesFound = LogFileDetector.DetectLogFileIn( logitem.LogDir, logitem.Filespec, logitem.MailDate );

			LogitIfThereWereNofilesFound( logitem, filesFound );

			foreach ( var file in filesFound )
			{
				var errorMsg = MailMan.SendMail( message: "Recently Added", subject: SubjectLine( file ), attachment: file );

				lastDate = LogResult( logitem, lastDate, file, errorMsg );
			}
			return lastDate;
		}

		private static string SubjectLine( string file )
		{
			return "New additions";
		}

		private static string SubjectLine()
		{
			return "New additions";
		}

		private DateTime LogResult( LogItem logitem, DateTime lastDate, string file, string errorMsg )
		{
			if ( string.IsNullOrEmpty( errorMsg ) )
			{
				lastDate = LogFileDetector.FileDate( logitem.LogDir, LogFileDetector.FilePartFile( logitem.LogDir, file ) );
				LogsMailed++;
				Logger.Info( string.Format( "Emailed {0}", file ) );
			}
			else
			{
				Logger.Error( string.Format( "Failed to email {0} - {1}", file, errorMsg ) );
			}
			return lastDate;
		}

		private void LogitIfThereWereNofilesFound( LogItem logitem, List<string> filesFound )
		{
			if ( NoFilesWereFound( filesFound ) )
			{
				Logger.Info(
				   string.Format( "  No Files were found in {0} like {1} later than {2}",
				   logitem.LogDir, logitem.Filespec, logitem.MailDate ) );
			}
		}

		private static bool NoFilesWereFound( List<string> filesFound ) => !filesFound.Any();
	}
}