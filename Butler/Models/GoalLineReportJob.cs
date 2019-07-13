using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class GoalLineReportJob : Job
	{
		public RosterGridReport Report { get; set; }

		public GoalLineReportJob( IKeepTheTime timekeeper )
		{
			Name = "GoalLine Report";
			Report = new GoallineReport( timekeeper );
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
            base.IsTimeTodo( out whyNot );

            if (!TimeKeeper.IsItRegularSeason())
                whyNot = "The Season hasnt started yet";
            
            if ( string.IsNullOrEmpty( whyNot ) )
				//  check if there is any new data
				whyNot = Report.CheckLastRunDate();
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( TimeKeeper.IsItPeakTime() )
					whyNot = "Peak time - no noise please";
			}
			if ( string.IsNullOrEmpty(whyNot)
                && TimeKeeper.IsItTuesday() )
				whyNot = "Not on Tuesdays";

			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}
