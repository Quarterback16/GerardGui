using RosterLib.Interfaces;
using NLog;
using RosterLib;
using RosterLib.RosterGridReports;
using System;

namespace Butler.Models
{
   public class PickupChartJob : Job
   {
      public int Week { get; set; }

      public RosterGridReport Report { get; set; }

      public PickupChartJob( IKeepTheTime timekeeper )
      {
         Name = "Pickup Chart";
         Console.WriteLine( "Constructing {0} ...", Name );
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
         Week = TimeKeeper.CurrentWeek( DateTime.Now );
         if (Week == 0) Week = 1;  //  in preseason lets look ahead to the first game
         Report = new PickupChart( 
            TimeKeeper.CurrentSeason( DateTime.Now ), Week );
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} job..............................................", Name );
         Report.RenderAsHtml(); //  the method that does the work
         Report.Finish();
         return string.Format( "Rendered {0} to {1}", Report.Name, Report.OutputFilename() );
      }

      public override bool IsTimeTodo( out string whyNot )
      {
         whyNot = string.Empty;
         if ( OnHold() ) whyNot = "Job is on hold";
         if (!string.IsNullOrEmpty( whyNot )) return ( string.IsNullOrEmpty( whyNot ) );
         //  check if there is any new data
         whyNot = Report.CheckLastRunDate();
         if (TimeKeeper.IsItPeakTime())
            whyNot = "Peak time - no noise please";
         if (!string.IsNullOrEmpty( whyNot )) return ( string.IsNullOrEmpty( whyNot ) );
         if ( !TimeKeeper.IsItFridaySaturdayOrSunday( System.DateTime.Now ) )
            whyNot = "Its not Friday Saturday or Sunday";
         return ( string.IsNullOrEmpty( whyNot ) );
      }

   }
}