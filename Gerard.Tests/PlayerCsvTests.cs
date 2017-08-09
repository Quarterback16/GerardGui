using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class PlayerCsvTests
	{
		[Ignore]  // fails speed test
		[TestMethod]
		public void TestDoPlayerCsvJob()  // 75 mins  2016-08-08
		{
			var sut = new PlayerCsvJob( new TimeKeeper( null ) );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestTimeToDoPlayerCsvJob()
		{
			var sut = new PlayerCsvJob( new FakeTimeKeeper( season: "2016", week: "12" ) );
			var outcome = sut.IsTimeTodo( out string whyNot );
			Assert.IsFalse( outcome );
			Assert.IsFalse( string.IsNullOrEmpty( whyNot ) );
		}

		[TestMethod]
		public void TestScoresPerYear()
		{
			var sut = new NFLPlayer( "MANNPE01" );
			var s = sut.ScoresPerYear();
			var testStr = s.ToString();
			var decSpot = testStr.IndexOf( '.' );
			var numDecPoints = testStr.Length - decSpot - 1;
			Assert.IsTrue( numDecPoints.Equals( 2 ) );
		}

		//  Test getting a players projections for a year
		[TestMethod]
		public void TestGetPlayerProjections()
		{
			var sut = new DbfPlayerGameMetricsDao();
			var pgms = sut.GetSeason( "2014", "KAEPCO01" );
			Assert.IsTrue( pgms.Count > 0 );
		}

		[TestMethod]
		public void TestRatePlayerProjection()
		{
			var p = new NFLPlayer( "TAYLTY01" );
			var sut = new DbfPlayerGameMetricsDao();
			var pgms = sut.GetSeason( "2016", "TAYLTY01" );
			var totalPoints = 0.0M;
			foreach ( var pgm in pgms )
			{
				pgm.CalculateProjectedFantasyPoints( p );
				totalPoints += p.Points;
			}
			Assert.IsTrue( p.Points < 400M );
		}

		[TestMethod]
		public void TestGameCodeGet()
		{
			var sut = new NFLWeek( "2014", 1 );
			var gameCode = sut.GameCodeFor( "DB" );
			Assert.IsTrue( gameCode.Equals( "2014:01-N" ) );
		}

		[TestMethod]
		public void TestPmetricsGet()
		{
			var player = new NFLPlayer( "MANNPE01" );
			var week = new NFLWeek( "2014", 1 );
			var gameCode = week.GameCodeFor( "DB" );
			var dao = new DbfPlayerGameMetricsDao();
			var pgm = dao.GetPlayerWeek( gameCode, player.PlayerCode );

			Assert.IsTrue( pgm.ProjYDp.Equals( 300 ) );
		}

		[TestMethod]
		public void TestGS4Scorer()
		{
			var player = new NFLPlayer( "MANNPE01" );
			var week = new NFLWeek( "2014", 1 );
			var sut = new GS4Scorer( week );
			var score = sut.RatePlayer( player, week );
			Assert.IsTrue( score.Equals( 9.0M ) );
		}

		[TestMethod]
		public void TestYahooScorer()
		{
			var player = new NFLPlayer( "MANNPE01" );
			var week = new NFLWeek( "2014", 1 );
			var sut = new YahooScorer( week );
			var score = sut.RatePlayer( player, week );
			Assert.IsTrue( score.Equals( 21.0M ) );
		}

		[TestMethod]
		public void TestYahooScorerLuck()
		{
			var player = new NFLPlayer( "LUCKAN01" );
			var week = new NFLWeek( "2014", 14 );
			var sut = new YahooScorer( week );
			var score = sut.RatePlayer( player, week );
			Assert.IsTrue( score.Equals( 24.0M ) );
		}

		[TestMethod]
		public void TestYahooScorerLuckLastScores()
		{
			//  Luck ran one in in Week 14
			var plyr = new NFLPlayer( "LUCKAN01" );
			var ds = plyr.LastScores( "R", 14, 14, "2014", "1" );
			var nScores = ds.Tables[ 0 ].Rows.Count;
			Assert.IsTrue( nScores.Equals( 1 ) );
		}
	}
}