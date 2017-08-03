﻿using NLog;
using RosterLib;
using RosterLib.Interfaces;

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
			var fileOut = string.Empty;
			var theWeek = new NFLWeek( TimeKeeper.Season, TimeKeeper.Week );
			foreach ( NFLGame game in theWeek.GameList() )
			{
				var summary = new GameSummary( game );
				summary.Render();
				fileOut = summary.FileName();
			}
			var finishedMessage = $"Finished {Name}";
			return finishedMessage;
		}

		public override bool IsTimeTodo( out string whyNot )
		{
			whyNot = string.Empty;
			base.IsTimeTodo( out whyNot );

			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( !TimeKeeper.IsItRegularSeason() )
					whyNot = "Its not the regular season yet";
			}

			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}