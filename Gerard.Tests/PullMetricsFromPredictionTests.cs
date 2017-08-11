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
				Player = new FakeNFLPlayer( "??01", "", "", "Unknown soldier" ),
				Game = g,
				Prediction = g.GetPrediction( "unit" )
			};
		}

		#endregion

		[TestMethod]
		public void PullMetricsFromPredictionProcess()
		{
			//  Processing happens in the constructor, bit smelly
			var sut = new PullMetricsFromPrediction( msg );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count > 0 );
		}

		[TestMethod]
		public void FakeDataHasAPrediction()
		{
			Assert.IsNotNull( msg.Prediction );
			Assert.IsTrue( msg.Prediction.HomeScore + msg.Prediction.AwayScore > 0 );
		}

		#region Running Backs

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

		#endregion

		#region  Passing

		[TestMethod]
		public void FakeDataHasValidHomePassUnit()
		{
			Assert.IsFalse( msg.Game.HomeNflTeam.PassUnit.HasIntegrityError() );
		}

		[TestMethod]
		public void FakeDataHasValidAwayPassUnit()
		{
			Assert.IsFalse( msg.Game.AwayNflTeam.PassUnit.HasIntegrityError() );
		}

		[TestMethod]
		public void FakeHomeQBGetsAllThePassingYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "QB01" );
			var projYDp = pgm.ProjYDp;
			Assert.AreEqual( expected: 430, actual: projYDp );
		}

		[TestMethod]
		public void FakeHomeQBGetsAllThePassingTDs()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "QB01" );
			var projTDp = pgm.ProjTDp;
			Assert.AreEqual( expected: 3, actual: projTDp );
		}

		[TestMethod]
		public void FakeHomeW1Gets40PerCentOfYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "WR01" );
			var projYDc = pgm.ProjYDc;
			Assert.AreEqual( expected: 172, actual: projYDc );
		}

		[TestMethod]
		public void FakeHomeW2Gets25PerCentOfYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "WR02" );
			var projYDc = pgm.ProjYDc;
			Assert.AreEqual( expected: 107, actual: projYDc );
		}

		[TestMethod]
		public void FakeHomeW3Gets10PerCentOfYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "WR03" );
			var projYDc = pgm.ProjYDc;
			Assert.AreEqual( expected: 43, actual: projYDc );
		}

		[TestMethod]
		public void FakeHomeTEGets20PerCentOfYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "TE01" );
			var projYDc = pgm.ProjYDc;
			Assert.AreEqual( expected: 86, actual: projYDc );
		}

		#endregion


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