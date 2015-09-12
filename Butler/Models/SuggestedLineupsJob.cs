using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
   public class SuggestedLineupsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public SuggestedLineupsJob(IKeepTheTime timekeeper)
      {
         Name = "Suggested Lineups";
         Report = new SuggestedLineup();
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} job..............................................", Name );
         Report.RenderAsHtml(); //  the old method that does the work
         Report.Finish();
         var completiomMsg = string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
         Logger.Info( completiomMsg );
         return completiomMsg;
      }

      public override bool IsTimeTodo(out string whyNot)
      {
         whyNot = string.Empty;

         base.IsTimeTodo(out whyNot);

#if ! DEBUG
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
#endif
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }
   }
}
