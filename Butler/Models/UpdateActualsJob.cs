using NLog;
using RosterLib;
using RosterLib.Interfaces;
using RosterLib.RosterGridReports;
using System;

namespace Butler.Models
{
	/// <summary>
	///   Updates the PGMETRICS table
	/// </summary>
	public class UpdateActualsJob : Job
	{
		public RosterGridReport Report { get; set; }

		public UpdateActualsJob( IKeepTheTime timeKeeper )
		{
			Name = "Update Actual Player Game Metrics";
			Report = new MetricsUpdateReport( timeKeeper );   //  do this
			TimeKeeper = timeKeeper;
			Report.TimeKeeper = timeKeeper;
			Logger = LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
		}

		public override string DoJob()
		{
			return Report.DoReport();
		}

		//  new business logic as to when to do the job
		public override bool IsTimeTodo( out string whyNot )
		{
			whyNot = string.Empty;
			if ( OnHold() ) whyNot = "Job is on hold";
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( !TimeKeeper.IsItRegularSeason() )
					whyNot = "Its not the Regular Season yet";
			}
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( TimeKeeper.IsItPeakTime() )
					whyNot = "Peak time - no noise please";

				if ( string.IsNullOrEmpty( whyNot ) )
				{
					if ( !TimeKeeper.IsItWednesdayOrThursday( DateTime.Now ) )
						whyNot = "Its not Wednesday or Thursday";
				}
			}

			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );

			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}