using RosterLib.Interfaces;
using RosterLib;

namespace Butler.Models
{
   public class DefensiveReportsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public DefensiveReportsJob( IKeepTheTime timekeeper )
      {
         Name = "Defensive Reports";
         Report = new DefensiveScorer();
         TimeKeeper = timekeeper;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         return Report.DoReport();
      }

      public override bool IsTimeTodo( out string whyNot )
      {
         whyNot = string.Empty;

         base.IsTimeTodo(out whyNot);
         if (string.IsNullOrEmpty(whyNot))
         {
            if (!TimeKeeper.IsItRegularSeason())
               whyNot = "Its not the regular season yet";
         }
         if ( string.IsNullOrEmpty( whyNot ) )
         {
#if ! DEBUG2
            if ( TimeKeeper.IsItPeakTime() )
               whyNot = "Peak time - no noise please";
            if ( string.IsNullOrEmpty( whyNot ) )
            {
               //  check if there is any new data
               whyNot = Report.CheckLastRunDate();
            }
#endif
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return ( string.IsNullOrEmpty( whyNot ) );
      }
   }
}