using RosterLib.Interfaces;
using NLog;
using RosterLib;
using System;

namespace Butler.Models
{
   public class FantasyProjectionJob : Job
   {
      public RosterGridReport Report { get; set; }

      public FantasyProjectionJob(IKeepTheTime timeKeeper)
      {
         TimeKeeper = timeKeeper;
         Name = "Fantasy Point Projections";
         Console.WriteLine("Constructing {0} ...", Name);
         Report = new FantasyProjectionReporter( TimeKeeper );
         Logger = LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} job..............................................", Name );
         StartRun();
         Report.RenderAsHtml(); //  the old method that does the work
         Report.Finish();
         StopRun();
         var completionMsg = string.Format( "Rendered {0} to {1}", Report.Name, Report.OutputFilename() );
         Logger.Info( "  {0}",completionMsg );
         return completionMsg;
      }

      //  new business logic as to when to do the job
      public override bool IsTimeTodo(out string whyNot)
      {
         base.IsTimeTodo( out whyNot );
         if (string.IsNullOrEmpty(whyNot))
         {
            if (!TimeKeeper.IsItRegularSeason())
               whyNot = "Its not the regular season yet";
         }
         if (string.IsNullOrEmpty(whyNot))
         {
            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
            if (string.IsNullOrEmpty(whyNot))
            {
               //  check if there is any new data
               whyNot = Report.CheckLastRunDate();
            }
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }
   }
}