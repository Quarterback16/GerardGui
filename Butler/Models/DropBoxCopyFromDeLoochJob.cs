using RosterLib;
using System;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class DropBoxCopyFromDeLoochJob : Job
	{
		public string SourceDir { get; set; }

		public string DestDir { get; set; }

		public DropBoxCopyFromDeLoochJob( IKeepTheTime timeKeeper,
		   string sourceDir,  //  "\\\\DeLooch\\users\\steve\\lists\\"
		   string destDir )   //  "d:\\shares\\public\\dropbox\\"
		{
			var theSeason = timeKeeper.Season;
			Name = "Get Dropbox list files from DeLooch";
			SourceDir = sourceDir;
			DestDir = destDir;
			Logger = NLog.LogManager.GetCurrentClassLogger();
		}

		public override string DoJob()
		{
			var outcome = FileUtility.CopyDirectory( SourceDir, DestDir );
			if ( string.IsNullOrEmpty( outcome ) )
			{
				var finishMessage = string.Format( "Copied {0} to {1}", SourceDir, DestDir );
				Logger.Info( "  {0}", finishMessage );
				return finishMessage;
			}

			Logger.Error( outcome );
			return outcome;
		}

		public override bool IsTimeTodo( out string whyNot )
		{

			base.IsTimeTodo( out whyNot );
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				whyNot = "";
			}
			if ( ! TimeKeeper.IsItPeakTime()  )
			{
				whyNot = "This job only runs in peak time (needs Delooch)";
			}
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}

	}
}
