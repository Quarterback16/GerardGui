using NLog;
using RosterLib;
using RosterLib.Interfaces;
using RosterLib.RosterGridReports;
using System;

namespace Butler.Models
{
	public class PickupChartJob : Job
	{
		public int Week { get; set; }

		public RosterGridReport Report { get; set; }

		public PickupChartJob( IKeepTheTime timekeeper, bool previous = false )
		{
			Name = "Pickup Chart";
			TimeKeeper = timekeeper;
			Logger = LogManager.GetCurrentClassLogger();
			Week = previous ? Int32.Parse( TimeKeeper.PreviousWeek() ) 
				: TimeKeeper.CurrentWeek( DateTime.Now );
			if ( Week == 0 ) Week = 1;  //  in preseason lets look ahead to the first game

			Report = new PickupChart( TimeKeeper, Week );
		}

		public override string DoJob()
		{
			Report.RenderAsHtml(); //  the method that does the work
			Report.Finish();
			return $"Rendered {Report.Name} to {Report.OutputFilename()}";
		}

		public override bool IsTimeTodo( out string whyNot )
		{
			whyNot = string.Empty;
			if ( OnHold() ) whyNot = "Job is on hold";
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				//  check if there is any new data
				whyNot = Report.CheckLastRunDate();
				if ( TimeKeeper.IsItPeakTime() )
					whyNot = "Peak time - no noise please";
			}
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}