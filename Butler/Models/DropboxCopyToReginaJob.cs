using RosterLib;
using System;

namespace Butler.Models
{
   public class DropboxCopyToReginaJob : Job
   {
      public string SourceDir { get; set; }

      public string DestDir { get; set; }

      public DropboxCopyToReginaJob()
		{
			Name = "Publish Dropbox to Regina";
			Console.WriteLine("Constructing {0} ...", Name);
			SourceDir = "d:\\shares\\public\\dropbox\\gridstat\\2015";
			DestDir = "\\\\Regina\\web\\medialists\\dropbox\\gridstat\\2015";
         Logger = NLog.LogManager.GetCurrentClassLogger();
		}
      public override string DoJob()
      {
         Logger.Info("Doing {0} job..............................................", Name);

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
