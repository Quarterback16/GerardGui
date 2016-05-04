using System;
using RosterLib;

namespace Butler.Models
{
   public class MediaListsPublishJob : Job
   {
      public string SourceDir { get; set; }

      public string DestDir { get; set; }

      public MediaListsPublishJob()
		{
			Name = "Publish Media Lists";
			SourceDir = "d:\\shares\\public\\dropbox\\medialists";
			DestDir = "\\\\Regina\\web\\medialists";
         Logger = NLog.LogManager.GetCurrentClassLogger();
		}

      public override string DoJob()
      {
         var outcome = FileUtility.CopyDirectory(SourceDir, DestDir);
         if (string.IsNullOrEmpty(outcome))
         {
            var finishedMessage = string.Format("Copied {0} to {1}", SourceDir, DestDir);
            Logger.Info("  {0}", finishedMessage);
            return finishedMessage;
         }
         Logger.Error(outcome);
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
