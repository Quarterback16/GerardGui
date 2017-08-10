using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class PullMetricsFromPredictionTests
	{
		#region  cut Initialisation

		private PlayerGameProjectionMessage msg;

		[TestInitialize]
		public void TestInitialize()
		{
			var g = new FakeNFLGame();
			msg = new PlayerGameProjectionMessage()
			{
				Player = new FakeNFLPlayer( "", "", "Unkown soldier" ),
				Game = g,
				Prediction = g.GetPrediction( "unit" )
			};
		}

		#endregion

		[TestMethod]
		public void TestPullMetricsFromPredictionProcess()
		{
			//  Processing happens in the constructor, bit smelly
			var sut = new PullMetricsFromPrediction( msg );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count > 0 );
		}

		[TestMethod]
		public void TestFakeDataHasAPrediction()
		{
			Assert.IsNotNull( msg.Prediction );
			Assert.IsTrue( msg.Prediction.HomeScore + msg.Prediction.AwayScore > 0 );
		}

		[TestMethod]
		public void TestFakeDataProducesAProjectionsRBs()
		{
			var sut = new PullMetricsFromPrediction( msg );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count == 2 );
		}

		[TestMethod]
		public void TestFakeDataHomeRB1ProjectsToHave70PercentOftheRushingYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var projYDr = msg.Game.PlayerGameMetrics[ 0 ].ProjYDr;
			Assert.AreEqual( expected: 78, actual: projYDr );
		}

		[TestMethod]
		public void TestFakeDataHomeAceProjectsToGetFirstTDr()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var projTDr = msg.Game.PlayerGameMetrics[ 0 ].ProjTDr;
			Assert.AreEqual( expected: 1, actual: projTDr );
		}

		[TestMethod]
		public void TestFakeDataHomeBackupProjectsToHave20PercentOftheRushingYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var projYDr = msg.Game.PlayerGameMetrics[ 1 ].ProjYDr;
			Assert.AreEqual( expected: 22, actual: projYDr );
		}

		[TestMethod]
		public void TestFakeDataHomeBackupProjectsToSecondTD()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var projTDr = msg.Game.PlayerGameMetrics[ 1 ].ProjTDr;
			Assert.AreEqual( expected: 1, actual: projTDr );
		}

		[Ignore]  //  its a slow integration test
		[TestMethod]
		public void TestJayCutler()
		{
			var g = new NFLGame( "2016:01-I" );
			var msg = new PlayerGameProjectionMessage()
			{
				Player = new NFLPlayer( "CUTLJA01" ),
				Game = g,
				Prediction = g.GetPrediction( "unit" )
			};
			var cut = new PullMetricsFromPrediction( msg );
			Assert.IsNotNull( msg.Game.PlayerGameMetrics.Count > 12 );
			msg.Dao = new DbfPlayerGameMetricsDao();
			var saveStep = new SavePlayerGameMetrics( msg );
		}
	}
}