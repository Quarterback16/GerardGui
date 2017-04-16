using NLog;
using RosterLib;
using RosterLib.Helpers;
using RosterLib.Interfaces;
using System.Collections;
using System.Linq;

namespace Butler.Models
{
	public class GeneratePlayerProjectionsJob : Job
	{
		public RosterGridReport Report { get; set; }

		public ISeasonScheduler SeasonScheduler { get; set; }

		public GeneratePlayerProjectionsJob( IKeepTheTime timeKeeper )
		{
			Name = "Generate Player Projections";
			TimeKeeper = timeKeeper;
			Logger = LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
			Report = new PlayerProjectionGenerator( timeKeeper, playerCache: null ) { Name = Name };
			SeasonScheduler = new SeasonScheduler();
		}

		public override string DoJob()
		{
			StartRun();
			var ppg = new PlayerProjectionGenerator( TimeKeeper, playerCache: null );
			var gameList = new ArrayList();

			//  do any unplayed games
			Logger.Debug( "   Doing whole season" );
			var s = new NflSeason( TimeKeeper.Season, loadGames: true, loadDivisions: false ); //  long time to load
			foreach ( var game in s.GameList )
				if ( !game.Played() )
					gameList.Add( game );

			var nGames = 0;
			foreach ( var game in gameList.Cast<NFLGame>() )
			{
				ppg.Execute( game );
				nGames++;
			}
			//  records will be in the PGMETRIC table

			StopRun();

			var finishedMessage = string.Format( "Generated projections for {0} games", nGames );
			Logger.Info( "  {0}", finishedMessage );
			return finishedMessage;
		}

		//  A weekly job, after new projections and before FP reports
		public override bool IsTimeTodo( out string whyNot )
		{
			whyNot = string.Empty;
			base.IsTimeTodo( out whyNot );
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( !SeasonScheduler.ScheduleAvailable( TimeKeeper.CurrentSeason() ) )
				{
					whyNot = "The schedule is not yet available for " + TimeKeeper.CurrentSeason();
				}
			}
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				whyNot = Report.CheckLastRunDate();
			}
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( TimeKeeper.IsItPeakTime() )
					whyNot = "Peak time - no noise please";
			}
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}