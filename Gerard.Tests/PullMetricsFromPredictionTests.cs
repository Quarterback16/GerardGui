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
				Player = new FakeNFLPlayer( "??01", "", "", "Unkown soldier" ),
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
		public void FakeDataHasValidHomeRunUnit()
		{
			var sut = new PullMetricsFromPrediction( msg );
			Assert.IsFalse( msg.Game.HomeNflTeam.RunUnit.HasIntegrityError());
		}

		[TestMethod]
		public void FakeDataHasValidAwayRunUnit()
		{
			var sut = new PullMetricsFromPrediction( msg );
			Assert.IsFalse( msg.Game.AwayNflTeam.RunUnit.HasIntegrityError() );
		}

		[TestMethod]
		public void TestFakeDataProducesFiveProjections()
		{
			var sut = new PullMetricsFromPrediction( msg );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count == 5);
		}

		[TestMethod]
		public void TestFakeDataHomeRB1ProjectsToHave70PercentOftheRushingYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var projYDr = msg.Game.PlayerGameMetrics[ 0 ].ProjYDr;
			Assert.AreEqual( expected: 78, actual: projYDr );
		}

		[TestMethod]
		public void TestFakeDataHAwayRB1ProjectsToHave70PercentOftheRushingYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var projYDr = msg.Game.PlayerGameMetrics[ 0 ].ProjYDr;
			Assert.AreEqual( expected: 78, actual: projYDr );
		}

		[TestMethod]
		public void TestFakeDataHomeAceProjectsToGetFirstTDr()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "JS01" );
			var projTDr = pgm.ProjTDr;
			Assert.AreEqual( expected: 1, actual: projTDr );
		}

		[TestMethod]
		public void TestFakeDataHomeBackupProjectsToHave20PercentOftheRushingYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "BB01" );
			var projYDr = pgm.ProjYDr;
			Assert.AreEqual( expected: 22, actual: projYDr );
		}

		[TestMethod]
		public void TestFakeDataHomeBackupProjectsToSecondTD()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "BB01" );  // backup, home team (TDr2) split
			var projTDr = pgm.ProjTDr;
			Assert.AreEqual( expected: 1, actual: projTDr );
		}

		[TestMethod]
		public void TestAwayAceProjectionAffectedByInjury()
		{
			var expected = (int) ( 82.0M * 0.7M ) ;
			var injChance = ( ( 3 * 10.0M ) / 100.0M );
			var effectiveness = 1 - injChance;
			expected = ( int ) ( expected * effectiveness );

			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "VV01" );
			var projYDr = pgm.ProjYDr;
			Assert.AreEqual( expected: expected, actual: projYDr );
		}

		[TestMethod]
		public void TestSecondAwayTDRGoesToTheVulture()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var vpgm = msg.GetPgmFor( "VU01" );
			var projVulturedTDr = vpgm.ProjTDr;
			Assert.AreEqual( expected: 1, actual: projVulturedTDr );
		}

		[TestMethod]
		public void TestFirstAwayTDRGoesToTheStarter()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "VV01" );
			var projTDr = pgm.ProjTDr;
			Assert.AreEqual( expected: 1, actual: projTDr );
		}

		[TestMethod]
		public void TestAwayBackupGetsZero()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "BB02" );
			var projTDr = pgm.ProjTDr;
			Assert.AreEqual( expected: 0, actual: projTDr );
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