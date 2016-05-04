using RosterLib;
using System;
using RosterLib.Interfaces;

namespace Butler.Models
{
   public class DropboxCopyToReginaJob : Job
   {
      public string SourceDir { get; set; }

      public string DestDir { get; set; }

      public DropboxCopyToReginaJob(IKeepTheTime timeKeeper)
		{
         var theSeason = timeKeeper.Season;
			Name = "Publish Dropbox to Regina";
			SourceDir = string.Format("d:\\shares\\public\\dropbox\\gridstat\\{0}", theSeason);
			DestDir = string.Format("\\\\Regina\\web\\medialists\\dropbox\\gridstat\\{0}", theSeason);
         Logger = NLog.LogManager.GetCurrentClassLogger();
		}
      public override string DoJob()
      {
         var outcome = FileUtility.CopyDirectory(SourceDir, DestDir);
         if (string.IsNullOrEmpty( outcome ))
         {
            var finishMessage = string.Format( "Copied {0} to {1}", SourceDir, DestDir );
            Logger.Info( "  {0}", finishMessage  );
            return finishMessage;
         }

         Logger.Error(outcome);
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
            //if (string.IsNullOrEmpty(whyNot))
            //{
            //   //  Is it already done? - check the date of the last backup
            //   //  check the datestamp of the control files if different backup!
            //   if (VesuviusControlFile() <= ReginaControlFile())
            //      whyNot = string.Format("Vesuvius date {0} sameas Regina Date {1}", VesuviusControlFile(), ReginaControlFile());
            //}
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }

      private DateTime ReginaControlFile()
      {
         var theDate = FileUtility.DateOf(string.Format("{0}\\tfl_ctl.dbf", DestDir));
         return theDate;
      }

      public DateTime VesuviusControlFile()
      {
         var theDate = FileUtility.DateOf(string.Format("{0}\\tfl_ctl.dbf", SourceDir));
         return theDate;
      }
   }
}
