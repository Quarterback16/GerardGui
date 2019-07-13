using NLog;
using RosterLib;
using RosterLib.Interfaces;
using System;

namespace Butler.Models
{
	public class GameSummariesJob : Job
	{
		public GameSummariesJob( IKeepTheTime timekeeper )
		{
			Name = "Game Summaries";
			TimeKeeper = timekeeper;
			Logger = LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
		}

		public override string DoJob()
		{
            var theWeek = new NFLWeek(
                seasonIn: TimeKeeper.Season,
                weekIn: TimeKeeper.Week );
			foreach ( NFLGame game in theWeek.GameList() )
			{
				var summary = new GameSummary( game );
				summary.Render();
                var fileOut = summary.FileName();
            }
			var finishedMessage = $"Finished {Name}";
			return finishedMessage;
		}

		public override bool IsTimeTodo( out string whyNot )
		{
            base.IsTimeTodo( out whyNot );

			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( !TimeKeeper.IsItRegularSeason() )
				{
					whyNot = "The Season hasnt started yet";
				}

				if ( string.IsNullOrEmpty( whyNot ) )
				{
					if ( TimeKeeper.CurrentWeek( DateTime.Now ) < 2 )
						whyNot = "Wait till the first week is over";
				}

				if ( string.IsNullOrEmpty( whyNot ) )
				{
					if ( TimeKeeper.IsItPeakTime() )
						whyNot = "Peak time - no noise please";
				}
			}

			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );

			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}
