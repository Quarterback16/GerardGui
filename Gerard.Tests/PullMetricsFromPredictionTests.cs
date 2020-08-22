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
		public void Committebacks_ShareTheLoad()
		{
			var g = new FakeNFLGame2();
			msg = new PlayerGameProjectionMessage()
			{
				Player = new FakeNFLPlayer( "??01", "", "", "Unknown soldier" ),
				Game = g,
				Prediction = g.GetPrediction( "unit" )
			};
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "CM02" );
			var projYDr = pgm.ProjYDr;
			Assert.AreEqual( expected: 50, actual: projYDr );
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
		public void TestFakeDataProducesManyProjections()
		{
			var sut = new PullMetricsFromPrediction( msg );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count > 5);
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
		public void FakeDataHomeBackupProjectsToSecondTD()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "BB01" );  // backup, home team (TDr2) split
			var projTDr = pgm.ProjTDr;
			Assert.AreEqual( expected: 1, actual: projTDr );
		}

		[TestMethod]
		public void FakeDataHomeBackupProjectsToGetSomeYDc()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "BB01" );  
			var projYDc = pgm.ProjYDc;
			Assert.AreEqual( expected: 21, actual: projYDc );
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
		public void QBGetsAllThePassingYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "QB01" );
			var projYDp = pgm.ProjYDp;
			Assert.AreEqual( expected: 430, actual: projYDp );
		}

		[TestMethod]
		public void QBGetsAllThePassingTDs()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "QB01" );
			var projTDp = pgm.ProjTDp;
			Assert.AreEqual( expected: 3, actual: projTDp );
		}

		[TestMethod]
		public void W1Gets35PerCentOfYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "WR01" );
			var projYDc = pgm.ProjYDc;
			Assert.AreEqual( expected: 150, actual: projYDc );
		}

		[TestMethod]
		public void W2Gets25PerCentOfYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "WR02" );
			var projYDc = pgm.ProjYDc;
			Assert.AreEqual( expected: 107, actual: projYDc );
		}

		[TestMethod]
		public void W3Gets15PerCentOfYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "WR03" );
			var projYDc = pgm.ProjYDc;
			Assert.AreEqual( expected: 64, actual: projYDc );
		}

		[TestMethod]
		public void TEGets20PerCentOfYards()
		{
			var sut = new PullMetricsFromPrediction( msg );
			var pgm = msg.GetPgmFor( "TE01" );
			var projYDc = pgm.ProjYDc;
			Assert.AreEqual( expected: 86, actual: projYDc );
		}

		#endregion

		//[Ignore]  //  its a slow integration test
		[TestMethod]
		public void TestPlayerProjection()
		{
			var g = new NFLGame( "2017:01-O" );
			var msg = new PlayerGameProjectionMessage()
			{
				Player = new NFLPlayer( "COOKDA01" ),
				Game = g,
				Prediction = g.GetPrediction( "unit" )
			};
			var cut = new PullMetricsFromPrediction( msg );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count > 12 );
		}

		[TestMethod]
		public void TestSaveStoresTheGameKey()
		{
			var g = new NFLGame( "2017:01-F" );
			var msg = new PlayerGameProjectionMessage()
			{
				Player = new NFLPlayer( "FOURLE01" ),
				Game = g,
				Prediction = g.GetPrediction( "unit" ),
				PlayerGameMetrics = new PlayerGameMetrics()
			};
			var cut = new PullMetricsFromPrediction( msg );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count > 12 );
		}

        [TestMethod]
        public void Test3DBackdoesnotLosesStats()
        {
            var g = new NFLGame("2020:01-F");
            var msg = new PlayerGameProjectionMessage()
            {
                Game = g,
                Prediction = g.GetPrediction("unit"),
                PlayerGameMetrics = new PlayerGameMetrics()
            };
            var cut = new PullMetricsFromPrediction(msg);
            //  game metrics should be updated
            Assert.IsTrue(msg.Game.PlayerGameMetrics.Count > 12);
            foreach (var item in msg.Game.PlayerGameMetrics)
            {
                System.Console.WriteLine(item);
            }
        }
    }
}