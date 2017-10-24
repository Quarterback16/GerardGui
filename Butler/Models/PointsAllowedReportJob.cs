using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class PointsAllowedReportJob : Job
	{
		public RosterGridReport Report { get; set; }

		public PointsAllowedReportJob( IKeepTheTime timekeeper )
		{
			Name = "Points Allowed Report";
			Report = new PointsAllowedReport( timekeeper );
			TimeKeeper = timekeeper;
			Logger = LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
		}

		public override string DoJob()
		{
			Report.RenderAsHtml(); //  the old method that does the work
			Report.Finish();
			var finishedMessage = $"Rendered {Report.Name} to {Report.OutputFilename()}";
			return finishedMessage;
		}

		//  new business logic as to when to do the job
		public override bool IsTimeTodo( out string whyNot )
		{
			whyNot = string.Empty;
			base.IsTimeTodo( out whyNot );

			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( !TimeKeeper.IsItRegularSeason() )
					whyNot = "Its not the regular season yet";
			}

			if ( string.IsNullOrEmpty( whyNot ) )
			{
				//  check if there is any new data
				whyNot = Report.CheckLastRunDate();
				if ( TimeKeeper.IsItPeakTime() )
					whyNot = "Peak time - no noise please";
			}

			if ( TimeKeeper.IsItTuesday() )
				whyNot = "Not on Tuesdays";

			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}