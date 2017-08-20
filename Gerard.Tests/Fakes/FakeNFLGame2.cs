using RosterLib;
using System.Collections.Generic;

namespace Gerard.Tests
{
	public class FakeNFLGame2 : NFLGame
	{
		public FakeNFLGame2()
		{
			//  fictional game AF @ BR
			Season = "2017";
			Week = "01";
			LoadFakeAwayTeam( "AF" );
			LoadFakeHomeTeam( "BR" );
			PlayerGameMetrics = new List<PlayerGameMetrics>();
		}

		private void LoadFakeAwayTeam(string awayTeamCode)
		{
			AwayNflTeam = new FakeNflTeam( awayTeamCode );
			var ru = new FakeRushUnit();
			ru.Load( awayTeamCode ); 
			AwayNflTeam.RunUnit = ru;

			var puAway = new FakePassUnit();
			puAway.Load( awayTeamCode );
			AwayNflTeam.PassUnit = puAway;
		}

		private void LoadFakeHomeTeam(string homeTeamCode)
		{
			HomeNflTeam = new FakeNflTeam(homeTeamCode);
			var homeru = new FakeRushUnit();
			homeru.Load( homeTeamCode );
			HomeNflTeam.RunUnit = homeru;

			var puHome = new FakePassUnit();
			puHome.Load( homeTeamCode );
			HomeNflTeam.PassUnit = puHome;
		}

		public override NFLResult GetPrediction( string method )
		{
			var dummyResult = new NFLResult()
			{
				HomeTeam = "BR",
				AwayTeam = "AF",
				HomeScore = 38,
				AwayScore = 34,

				HomeTDp = 3,
				HomeYDp = 430,
				HomeTDr = 2,
				HomeFg = 1,
				HomeTDd = 0,
				HomeTDs = 0,
				HomeYDr = 112,

				AwayTDp = 3,
				AwayYDp = 330,
				AwayTDr = 2,
				AwayFg = 3,
				AwayTDd = 0,
				AwayTDs = 0,
				AwayYDr = 82
			};
			return dummyResult;
		}
	}
}
