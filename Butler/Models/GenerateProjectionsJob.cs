using System.Collections;
using System.Linq;
using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class GeneratePlayerProjectionsJob : Job
	{
		public RosterGridReport Report { get; set; }

		public GeneratePlayerProjectionsJob( IKeepTheTime timeKeeper )
		{
			Name = "Player Projection Generator";
			TimeKeeper = timeKeeper;
			Logger = LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
			Report = new PlayerProjectionGenerator( playerCache: null ) {Name = Name};
			Report.SetLastRunDate();
		}

		public override string DoJob()
		{
			Logger.Info( "Doing {0} job..............................................", Name );
			StartRun();
			var ppg = new PlayerProjectionGenerator( playerCache: null );
			var gameList = new ArrayList();

			if (TimeKeeper.IsItPreseason())
			{
				// do the whole season
				Logger.Debug( "   Doing whole season" );
				var s = new NflSeason( Utility.CurrentSeason(), true, false ); //  long time to load
				foreach ( var game in s.GameList )
					gameList.Add( game );
			}
			else
			{
				//  do the upcoming week
				var w = new NFLWeek( TimeKeeper.Season, TimeKeeper.Week );
				gameList = w.GameList();
			}

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
			base.IsTimeTodo( out whyNot );
			if (string.IsNullOrEmpty( whyNot ))
			{
				whyNot = Report.CheckLastRunDate();
				if (string.IsNullOrEmpty( whyNot ))
				{
					if (string.IsNullOrEmpty( whyNot ))
					{
						if (TimeKeeper.IsItPeakTime())
							whyNot = "Peak time - no noise please";
					}
				}
			}
			if (!string.IsNullOrEmpty( whyNot ))
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}