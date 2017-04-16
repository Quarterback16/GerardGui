using System;
using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
   public class RunReportJob : ReportJob
   {
      public RunReportJob( IKeepTheTime timekeeper )
      {
         Name = "Run Report job";
         Report = new RunReport( timekeeper );
	     CheckLastRun = false;  //  dont restrict running to only when data updates
         TimeKeeper = timekeeper;
         Report.Season = TimeKeeper.CurrentSeason( DateTime.Now );
         Logger = LogManager.GetCurrentClassLogger();
      }

      public override bool IsTimeTodo( out string whyNot )
      {
         base.IsTimeTodo(out whyNot);

         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return ( string.IsNullOrEmpty( whyNot ) );
      }
   }
}