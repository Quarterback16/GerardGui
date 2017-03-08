using RosterLib.Interfaces;
using NLog;
using RosterLib;

namespace Butler.Models
{
   public class FantasyReportJob : ReportJob
   {
      public FantasyReportJob( IKeepTheTime timekeeper )
      {
         Name = "Fantasy Report";
         Report = new FantasyReport( timekeeper );
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override bool IsTimeTodo( out string whyNot )
      {
         whyNot = string.Empty;
         base.IsTimeTodo( out whyNot );
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if ( !TimeKeeper.IsItRegularSeason() )
               whyNot = "Its not the Regular Season";
         }
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            //  check if there is any new data
            whyNot = Report.CheckLastRunDate();
            if ( TimeKeeper.IsItPeakTime() )
            {
               whyNot = "Peak time - no noise please";
            }
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );

         return ( string.IsNullOrEmpty( whyNot ) );
      }
   }
}
