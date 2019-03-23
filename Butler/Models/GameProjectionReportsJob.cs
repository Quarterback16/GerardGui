using RosterLib;
using RosterLib.Helpers;
using RosterLib.Interfaces;
using System;

namespace Butler.Models
{
	public class GameProjectionReportsJob : Job
	{
		public RosterGridReport Report { get; set; }

        public ISeasonScheduler SeasonScheduler { get; set; }

        public GameProjectionReportsJob( IKeepTheTime timeKeeper )
		{
			Name = "Game Projection Reports";
			Report = new GameProjectionsReport( timeKeeper );
			TimeKeeper = timeKeeper;
			Logger = NLog.LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
            SeasonScheduler = new SeasonScheduler();
        }

		public override string DoJob()
		{
			return Report.DoReport();
		}

		public override bool IsTimeTodo( out string whyNot )
		{
			base.IsTimeTodo( out whyNot );

            if (!SeasonScheduler.ScheduleAvailable(
                TimeKeeper.CurrentSeason()))
            {
                whyNot = "The schedule is not yet available for "
                    + TimeKeeper.CurrentSeason();
            }

            if ( string.IsNullOrEmpty( whyNot ) )
			{
				//  check if there is any new data
				whyNot = Report.CheckLastRunDate();
			}
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( TimeKeeper.IsItPeakTime() )
					whyNot = $"{DateTime.Now.TimeOfDay:t} is peak time";
			}
			if ( TimeKeeper.IsItTuesday() )
				whyNot = "Not on Tuesdays";

			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}