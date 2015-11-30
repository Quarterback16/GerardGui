using RosterLib;
using System;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class DropBoxCopyTflToVesuviusJob : Job
	{
		public string SourceDir { get; set; }

		public string DestDir { get; set; }

		public DropBoxCopyTflToVesuviusJob(IKeepTheTime timeKeeper)
		{
			Name = "Reload Vesuvius TFL Data";
			Console.WriteLine("Constructing {0} ...", Name);
			SourceDir = "d:\\shares\\public\\dropbox\\tfl";
			DestDir = "d:\\shares\\tfl";
			TimeKeeper = timeKeeper;
			Logger = NLog.LogManager.GetCurrentClassLogger();
		}

		public override string DoJob()
		{
			Logger.Info("Doing {0} job..............................................", Name);

			string outcome;
			if (FileUtility.CountFilesInDirectory(SourceDir) > 0)
			{
				outcome = FileUtility.CopyDirectory(SourceDir, DestDir);
				if (!string.IsNullOrEmpty(outcome)) return outcome;

				var finishMessage = string.Format("Copied {0} to {1}", SourceDir, DestDir);
				Logger.Info("  {0}", finishMessage);
				FileUtility.DeleteAllFilesInDirectory(SourceDir);
				return finishMessage;
			}
			else
				outcome = "No files available";

			return outcome;
		}

		public override bool IsTimeTodo(out string whyNot)
		{
			whyNot = string.Empty;
			if (OnHold()) whyNot = "Job is on hold";
			if (!string.IsNullOrEmpty(whyNot)) return (string.IsNullOrEmpty(whyNot));
			//  check if there is any new data
			if (TimeKeeper.IsItPeakTime())
				whyNot = "Peak time - no noise please";
			return string.IsNullOrEmpty(whyNot);
		}

	}
}
