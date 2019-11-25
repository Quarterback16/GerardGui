using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class PerformanceReportJob : ReportJob
	{
		public PerformanceReportJob( IKeepTheTime timekeeper )
		{
			Name = "Performance Report job";
			Report = new PerformanceReportGenerator( timekeeper );
			TimeKeeper = timekeeper;
			Logger = LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
		}

		public override bool IsTimeTodo( out string whyNot )
		{
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
			if ( TimeKeeper.IsItTuesday() )
				whyNot = "Not on Tuesdays";
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );

			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}