using NLog;
using RosterLib;
using System;
using RosterLib.Interfaces;

namespace Butler.Models
{
   public class DepthChartJob : Job
   {
      public RosterGridReport Report { get; set; }

      public DepthChartJob( IKeepTheTime timekeeper )
      {
         Name = "Depth Charts";
         Console.WriteLine("Constructing {0} ...", Name);
         Report = new DepthChartReport();
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} job..............................................", Name );
         Report.RenderAsHtml(); //  the old method that does the work
         Report.Finish();
         var finishedMessage = string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
         Logger.Info( "  {0}", finishedMessage  );
         return finishedMessage;
      }

      //  new business logic as to when to do the job
      public override bool IsTimeTodo(out string whyNot)
      {
         base.IsTimeTodo(out whyNot);
         if (string.IsNullOrEmpty(whyNot))
         {
            if (string.IsNullOrEmpty(whyNot))
            {
               //  check if there is any new data
               whyNot = Report.CheckLastRunDate();
               if (TimeKeeper.IsItPeakTime())
                  whyNot = "Peak time - no noise please";
            }
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }
   }
}