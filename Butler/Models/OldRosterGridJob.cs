using RosterLib;
using System;
using RosterLib.Interfaces;

namespace Butler.Models
{
   public class OldRosterGridJob : Job
   {
      public RosterGridReport Report { get; set; }

      public OldRosterGridJob(IKeepTheTime timeKeeper )
      {
         Name = "Old Roster Grid";
         Console.WriteLine("Constructing {0} ...", Name);
         Report = new OldRosterGridReports();
         TimeKeeper = timeKeeper;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} job..............................................", Name );
         StartRun();
         Report.RenderAsHtml(); //  the old method that does the work
         Report.Finish();
         StopRun();
         var finishMessage = string.Format( "Rendered {0} to {1}", Report.Name, Report.OutputFilename() );
         Logger.Info( "  {0}", finishMessage  );
         return finishMessage;
      }

      //  new business logic as to when to do the job
      public override bool IsTimeTodo( out string whyNot )
      {
         whyNot = string.Empty;
         base.IsTimeTodo(out whyNot);

         if ( string.IsNullOrEmpty( whyNot ) )
         {
            //  check if there is any new data
            whyNot = Report.CheckLastRunDate();
            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return ( string.IsNullOrEmpty( whyNot ) );
      }
   }
}