using Helpers;
using Helpers.Interfaces;
using Helpers.Models;
using System;
using System.Collections.Generic;

namespace Butler.Models
{
	public class LogMailerJob : Job
	{
		private const string K_RecipientsKey = "LogRecipients";

		public int LogsMailed { get; set; }

		protected IMailMan MailMan { get; set; }

		protected IDetectLogFiles LogFileDetector { get; set; }

		protected LogMaster LogMaster { get; set; }

		public LogMailerJob( IMailMan mailMan, IDetectLogFiles logFileDetector )
		{
			Name = "Log Mailer";
			Logger = NLog.LogManager.GetCurrentClassLogger();
			MailMan = mailMan;
			LogMaster = new LogMaster( ".\\xml\\mail-list.xml" );
			LogFileDetector = logFileDetector;
		}

		public override string DoJob()
		{
			LogsMailed = 0;

			var lastDate = new DateTime( 1, 1, 1 );

			var keys = new List<string>();
			foreach ( System.Collections.DictionaryEntry de in LogMaster.TheHt )
				keys.Add( de.Key.ToString() );

			//  for each log item in the XML
			foreach ( string key in keys )
			{
				var logitem = ( LogItem ) LogMaster.TheHt[ key ];
                //DumpLogItem(logitem);
                lastDate = MailLogFiles( logitem );
				//  mark as done
				if ( lastDate != new DateTime( 1, 1, 1 ) )
				{
					logitem.MailDate = lastDate;
					LogMaster.PutItem( logitem );
				}
			}

			LogMaster.Dump2Xml();

			var finishedMessage = $"  {Name} job - done. {LogsMailed} logs mailed";
			Logger.Info( finishedMessage );
			return finishedMessage;
		}

        private void DumpLogItem(LogItem logitem)
        {
            Logger.Trace($"LogDir:{logitem.LogDir}");
            Logger.Trace($"FileSpec:{logitem.Filespec}");
            Logger.Trace($"MailDate:{logitem.MailDate}");
            Logger.Trace($"Recipients:{logitem.Recipients}");
            Logger.Trace($"Subject:{logitem.Subject}");
        }

        private DateTime MailLogFiles( LogItem logitem )
		{
			var lastDate = new DateTime( 1, 1, 1 );
			var filesFound = LogFileDetector.DetectLogFileIn(
                logitem.LogDir, 
                logitem.Filespec,
                logitem.MailDate );
			Logger.Trace( $"Found {filesFound.Count} file(s)" );
			foreach ( var file in filesFound )
			{
				MailMan.AddRecipients( logitem.Recipients );
				Logger.Trace( $"There are {MailMan.RecipientCount()} recipients" );
				var errorMsg = MailMan.SendMail(
					message: "Log file",
					subject: logitem.Subject,
					attachment: file );
				if ( string.IsNullOrEmpty( errorMsg ) )
				{
					lastDate = LogFileDetector.FileDate(
						logitem.LogDir,
						LogFileDetector.FilePartFile( logitem.LogDir, file ) );
					LogsMailed++;
					Logger.Trace( $"Emailed {file} ({logitem.Subject})" );
				}
				else
				{
					Logger.Error( $"Failed to email {file} - {errorMsg}" );
				}
				MailMan.ClearRecipients();
			}
			return lastDate;
		}
	}
}