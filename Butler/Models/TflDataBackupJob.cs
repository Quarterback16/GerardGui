using RosterLib;
using System;

namespace Butler.Models
{
	public class TflDataBackupJob : Job
	{
		public string SourceDir { get; set; }

		public string DestDir { get; set; }

		public TflDataBackupJob()
		{
			Name = "TFL DATA Backup";
			Console.WriteLine("Constructing {0} ...", Name);
			SourceDir = "d:\\shares\\tfl";
			DestDir = "\\\\Regina\\Documents\\Backup\\tfl";
         Logger = NLog.LogManager.GetCurrentClassLogger();
		}

		public override string DoJob()
		{
         Logger.Info( "Doing {0} job..............................................", Name );
			//  copy tfl dir to Regina

			// To copy a file to another location and
			// overwrite the destination file if it already exists.
			var outcome = FileUtility.CopyDirectory(SourceDir, DestDir);
         if ( string.IsNullOrEmpty( outcome ) )
            return string.Format( "Copied {0} to {1}", SourceDir, DestDir );
		   Logger.Error( outcome );
		   return outcome;
		}

		public override bool IsTimeTodo(out string whyNot)
		{
		   base.IsTimeTodo(out whyNot);
			if (string.IsNullOrEmpty(whyNot))
			{
#if DEBUG
				whyNot = "In Dev mode";
#endif
				if (string.IsNullOrEmpty(whyNot))
				{
					//  Is it already done? - check the date of the last backup
					//  check the datestamp of the control files if different backup!
					if (VesuviusControlFile() <= ReginaControlFile())
						whyNot = string.Format("Vesuvius date {0} sameas Regina Date {1}", VesuviusControlFile(), ReginaControlFile());
				}
			}
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return (string.IsNullOrEmpty(whyNot));
		}

		private DateTime ReginaControlFile()
		{
			var theDate = FileUtility.DateOf(string.Format("{0}\\nfl\\player.dbf", DestDir));
			return theDate;
		}

		public DateTime VesuviusControlFile()
		{
			var theDate = FileUtility.DateOf( string.Format( "{0}\\nfl\\player.dbf", SourceDir ) );
			return theDate;
		}
	}
}