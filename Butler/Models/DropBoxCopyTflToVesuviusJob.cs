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
			Logger.Info("Copying files from {0} to {1}", SourceDir, DestDir);

			string outcome;
			var fileCount = FileUtility.CountFilesInDirectory(SourceDir);
			if ( fileCount > 0)
			{
				outcome = FileUtility.CopyDirectory(SourceDir, DestDir);
				if (!string.IsNullOrEmpty(outcome)) return outcome;

				var finishMessage = string.Format("Copied {2} files from {0} to {1}", SourceDir, DestDir, fileCount);
				Logger.Info("  {0}", finishMessage);
				FileUtility.DeleteAllFilesInDirectory(SourceDir);
				return finishMessage;
			}
			outcome = "No files available";
			Logger.Info("  {0}", outcome);
			return outcome;
		}

		public override bool IsTimeTodo(out string whyNot)
		{
			whyNot = string.Empty;
			if (OnHold()) whyNot = "Job is on hold";
			return !string.IsNullOrEmpty(whyNot) ? (string.IsNullOrEmpty(whyNot)) : string.IsNullOrEmpty(whyNot);
		}

	}
}
