using RosterLib.Interfaces;
using RosterLib;
using System;

namespace Butler.Models
{
   public class StatGridJob : Job
   {
      public RosterGridReport Report { get; set; }

      public StatGridJob(IKeepTheTime timeKeeper)
      {
         Name = "Stat Grids";
         Console.WriteLine("Constructing {0} ...", Name);
         Report = new StatGrids();
         TimeKeeper = timeKeeper;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} job..............................................", Name );
         Report.RenderAsHtml(); //  the old method that does the work
         Report.Finish();
         return string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
      }

      //  new business logic as to when to do the job
      //  this needs to be done weekly during the season

      public override bool IsTimeTodo(out string whyNot)
      {
         whyNot = string.Empty;
         base.IsTimeTodo(out whyNot);
         if (string.IsNullOrEmpty(whyNot))
         {
            if (string.IsNullOrEmpty(whyNot))
            {
               if (TimeKeeper.IsItPreseason())
                  whyNot = "This only runs in Regular season";
               else if (TimeKeeper.IsItPeakTime())
                  whyNot = "Peak time - no noise please";
               if (string.IsNullOrEmpty(whyNot))
               {
                  //  check if there is any new data
                  whyNot = Report.CheckLastRunDate();
               }
            }
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );

         return (string.IsNullOrEmpty(whyNot));
      }
   }
}