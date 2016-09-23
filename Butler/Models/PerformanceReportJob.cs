using System;
using RosterLib.Interfaces;
using NLog;
using RosterLib;

namespace Butler.Models
{
   public class PerformanceReportJob : ReportJob
   {
      public PerformanceReportJob( IKeepTheTime timekeeper )
      {
         Name = "Performance Report job";
         Report = new PerformanceReportGenerator(timekeeper);
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override bool IsTimeTodo( out string whyNot )
      {
         whyNot = string.Empty;
         base.IsTimeTodo(out whyNot);
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
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if ( !TimeKeeper.IsItWednesdayOrThursday( DateTime.Now ) )
               whyNot = "Its not Wednesday or Thursday";
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );

         return ( string.IsNullOrEmpty( whyNot ) );
      }
   }
}
