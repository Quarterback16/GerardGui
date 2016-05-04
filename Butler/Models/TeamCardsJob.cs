using RosterLib.Interfaces;
using RosterLib;
using System;

namespace Butler.Models
{
   public class TeamCardsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public TeamCardsJob(IKeepTheTime keeper)
      {
         Name = "Team Cards";
         TimeKeeper = keeper;
         Report = new TeamCards(false);
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         return Report.DoReport();
      }

      //  new business logic as to when to do the job
      public override bool IsTimeTodo(out string whyNot)
      {
         whyNot = string.Empty;
         base.IsTimeTodo(out whyNot);
         if (string.IsNullOrEmpty(whyNot))
         {

            //  check if there is any new data
            whyNot = Report.CheckLastRunDate();


            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }
   }
}