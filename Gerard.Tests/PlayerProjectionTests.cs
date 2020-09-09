using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System.IO;

namespace Gerard.Tests
{
	[TestClass]
	public class PlayerProjectionTests
	{
		[TestMethod]
		public void TestPlayerProjection()  //  25 sec 2015-08-14
		{
			var pp = new PlayerProjection( "HUNDBR01", "2017" );
			pp.Render();
			var fileOut = pp.FileName();
			Assert.IsTrue( File.Exists( fileOut ), $"Cannot find {fileOut}" );
		}

		[TestMethod]
		public void TestPlayerIsAce()
		{
			var sut = new NFLPlayer( "IVORCH01" );
			Assert.IsFalse( sut.IsAce() );
		}

		[TestMethod]
		public void TestPlayerIsPartOfTwoHeadedMonster()
		{
			var sut = new NFLPlayer( "IVORCH01" );
			Assert.IsFalse( sut.IsTandemBack() );
		}

		[TestMethod]
		public void TestPlayerProjectionPeytonManning2014()
		{
			var pp = new PlayerProjection( "MANNPE01", "2014" );
			pp.Render();
			var fileOut = pp.FileName();
			Assert.IsTrue(
                File.Exists( fileOut ), $"Cannot find {fileOut}" );
		}

		[TestMethod]
		public void TestPlayerProjectionJaquizRodgers2015()
		{
			var pp = new PlayerProjection( "RODGJA01", "2015" );
			pp.Render();
			var fileOut = pp.FileName();
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}

		[TestMethod]
		public void TestPlayerProjectionMattForte2015()
		{
			var pp = new PlayerProjection(
                playerId: "FORTMA01",
                season: "2015");
			pp.Render();
			var fileOut = pp.FileName();
			Assert.IsTrue(
                File.Exists(fileOut),
                $"Cannot find {fileOut}");
		}

		[TestMethod]
		public void TestPlayerProjectionPrediction()
		{
			var g = new NFLGame( "2013:01-A" );  // YYYY:0W-X
			var prediction = g.GetPrediction( "unit" );
			Assert.IsNotNull( prediction );
			Assert.AreEqual( 0, prediction.AwayTDr, "Away TDr should be 0" );
			Assert.AreEqual( 1, prediction.AwayTDp, "Away TDp should be 1" );
			Assert.AreEqual( 1, prediction.AwayFg, "Away FG should be 1" );
		}

        [TestMethod]
        public void TestPlayerProjectionChrisCarson2020()
        {
            TestPlayer(
                "CARSCH01",
                "2020");
        }

        [TestMethod]
        public void TestPlayerProjectionDerrickHenry2020()
        {
            TestPlayer(
                "HENRDE01",
                "2020");
        }

        private static void TestPlayer(
            string playerId,
            string season)
        {
            var pp = new PlayerProjection(
                playerId: playerId,
                season: season);
            pp.Render();
            var fileOut = pp.FileName();
            Assert.IsTrue(
                File.Exists(fileOut),
                $"Cannot find {fileOut}");
        }
    }
}