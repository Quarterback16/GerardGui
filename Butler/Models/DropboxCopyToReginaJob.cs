using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class DropboxCopyToReginaJob : Job
	{
		public string SourceDir { get; set; }

		public string DestDir { get; set; }

		public DropboxCopyToReginaJob( IKeepTheTime timeKeeper,
		   string sourceDir,  //  "d:\\shares\\public\\dropbox\\gridstat\\{0}"
		   string destDir )   //  "\\\\Regina\\web\\medialists\\dropbox\\gridstat\\{0}"
		{
			var theSeason = timeKeeper.Season;
			Name = "Publish Dropbox to Regina";
			SourceDir = string.Format( sourceDir, theSeason );
			DestDir = string.Format( destDir, theSeason );
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

			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );

			return ( string.IsNullOrEmpty( whyNot ) );
		}

	}
}
