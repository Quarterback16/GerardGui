using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class PlayerProjectionGeneratorTests
	{
		[TestMethod]
		public void TestWeeklyProjection()
		{
			var w = new NFLWeek( "2014", "06" ); 
			var sut = new PlayerProjectionGenerator( new FakeTimeKeeper( season: "2014", week: "06" ), null);
			var nGames = 0;
			for ( var i = 0; i < w.GameList().Count; i++ )
			{
				var game = (NFLGame) w.GameList()[ i ];
				sut.Execute( game );
				nGames++;
			}
			Assert.IsTrue( nGames > 10 && nGames < 17 );
		}

		[TestMethod]
		public void TestProjectionOneGame()
		{
			var g = new NFLGame("2015:01-C");
			var sut = new PlayerProjectionGenerator( new FakeTimeKeeper( season: "2015", week: "01" ), null);
			sut.Execute(g);
			Assert.IsNotNull(sut);
		}

		[TestMethod]
		public void TestGettingGamePrediction()
		{
			var msg = new PlayerGameProjectionMessage {Game = new NFLGame( "2014:01-A" )};
		   var sut = new GetGamePrediction( msg );
			Assert.IsNotNull( msg.Prediction );
			Utility.Announce( msg.Prediction.PredictedScore() );
		}

		[TestMethod]
		public void TestPullMetrics()
		{
			var msg = new PlayerGameProjectionMessage {Game = new NFLGame( "2016:01-N" )};
			var sut = new GetGamePrediction(msg);
			var sut2 = new PullMetricsFromPrediction(msg);
			Assert.IsNotNull(sut2);
		}

		[TestMethod]
		public void TestAllocationToAce()
		{
			var msg = new PlayerGameProjectionMessage {Game = new NFLGame( "2013:01-B" )};
			var sut = new GetGamePrediction( msg );
			Assert.IsNotNull( msg.Prediction );
			Utility.Announce( msg.Prediction.PredictedScore() );
			var sut2 = new PullMetricsFromPrediction( msg );
			Assert.IsNotNull( msg.Game.PlayerGameMetrics );
			Assert.AreEqual( 1, msg.Game.PlayerGameMetrics.Count );
		}

		[TestMethod]
		public void TestASavingMetrics()
		{
			var msg = new PlayerGameProjectionMessage {Game = new NFLGame( "2013:01-B" )};
			var sut = new GetGamePrediction( msg );
			var sut2 = new PullMetricsFromPrediction( msg );
			var sut3 = new SavePlayerGameMetrics( msg );
			var dpgmDoa = new DbfPlayerGameMetricsDao();
			var pgmList = msg.Game.PlayerGameMetrics;
			var expectedPgm = pgmList.FirstOrDefault();
			var pgm = dpgmDoa.Get( expectedPgm.PlayerId, expectedPgm.GameKey );
			Assert.IsNotNull( pgm );
		}

		[TestMethod]
		public void TestAllocation()
		{
			var msg = new PlayerGameProjectionMessage {Game = new NFLGame( "2013:01-B" )};
			var sut = new GetGamePrediction( msg );
			Assert.IsNotNull( msg.Prediction );
			Utility.Announce( msg.Prediction.PredictedScore() );
			var sut2 = new PullMetricsFromPrediction( msg );
			Assert.IsNotNull( msg.Game.PlayerGameMetrics );
			var sut3 = new SavePlayerGameMetrics(msg);
		}

		[TestMethod]
		public void TestDeleteMetricsWorks()
		{
			Utility.TflWs.ClearPlayerGameMetrics( "2013:01-B" );
			var ds = Utility.TflWs.GetPlayerGameMetrics( "HOPKDU01", "2013:01-B" );
			Assert.IsTrue( ds.Tables[ 0 ].Rows.Count == 0 );
		}

		[TestMethod]
		public void TestGetMetricsWorks()
		{
			var ds = Utility.TflWs.GetPlayerGameMetrics("LYNCMA01", "2014:01-A");
			Assert.IsTrue(ds.Tables[0].Rows.Count > 0);
		}

		[TestMethod]
		public void TestInjury()
		{
			var p = new NFLPlayer( "GOREFR01" );  // has injury rating 1
			var injury = PullMetricsFromPrediction.AllowForInjuryRisk( p, 100 );
			Assert.IsTrue( injury == 90 );
		}

		[TestMethod]
		public void TestVulture()
		{
			var msg = new PlayerGameProjectionMessage {Game = new NFLGame( "2013:04-I" ) {PlayerGameMetrics = new List<PlayerGameMetrics>()}};
			var sut = new GetGamePrediction( msg );
			Assert.IsNotNull( msg.Prediction );
			Utility.Announce( msg.Prediction.PredictedScore() );
			var sut2 = new PullMetricsFromPrediction( msg );
			Assert.IsNotNull( msg.Game.PlayerGameMetrics );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count > 0 );
		}

	}
}
