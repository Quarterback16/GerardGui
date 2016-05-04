using Butler.Helpers;
using RosterLib;
using System;

namespace Butler.Models
{
	public class UpdateTflJob : Job
	{
		public string SourceDir { get; set; }

		public string DestDir { get; set; }

		public DiskDetector DiskDetector { get; set; }

		public UpdateTflJob()
		{
			Name = "TFL DATA Update";
			SourceDir = "\\\\Regina\\Documents\\Backup\\tfl";
			DestDir = "d:\\shares\\tfl";
			DiskDetector = new DiskDetector();
         Logger = NLog.LogManager.GetCurrentClassLogger();
		}

		public override string DoJob()
		{
			//  copy tfl dir to Vesuvius from the emtec drive

			// To copy a file to another location and
			// overwrite the destination file if it already exists.
			FileUtility.CopyDirectory(SourceDir, DestDir);
         var msg = string.Format("Copied {0} to {1}", SourceDir, DestDir);
         Logger.Info(msg);
         return msg;
		}

		public override bool IsTimeTodo(out string whyNot)
		{
			whyNot = string.Empty;
         base.IsTimeTodo(out whyNot);
			if (string.IsNullOrEmpty(whyNot))
			{
#if DEBUG
				whyNot = "In Dev mode";
#endif
				if (string.IsNullOrEmpty(whyNot))
				{
					//  Do it if you detect the Drive is Available
					if (!DiskDetector.IsDiskAvailable("<diskId>"))
					{
						whyNot = "Disk is not available " + DiskDetector.DiskIdentifiers();
					}
					//  check the datestamp of the control files if different backup!
				}
			}
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return (string.IsNullOrEmpty(whyNot));
		}
	}
}