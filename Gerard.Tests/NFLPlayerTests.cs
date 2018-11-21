using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class NFLPlayerTests
	{
		#region  cut Initialisation

		private NFLPlayer cut;

		[TestInitialize]
		public void TestInitialize()
		{
			cut = ClassUnderTest();
		}

		private static NFLPlayer ClassUnderTest()
		{
			return new NFLPlayer("MARIMA02");
		}

		#endregion

		[TestMethod]
		public void TestHealthRating()
		{
			var result = cut.HealthRating();
			Assert.AreEqual( expected: 0.1M, actual: result );
		}

		[TestMethod]
		public void TestActualPointsForDeshahnWatson()
		{
			var sut = new NFLPlayer("WATSDE02");
			var game = new NFLGame("2017:05-M");
			var result = sut.ActualFpts(game);
			Assert.AreEqual( expected: 35.5M, actual: result );
		}

        [TestMethod]
        public void Player_WhenScoredInLastGame_ReturnsTrue()
        {
            var sut = new NFLPlayer("MOORDA04");
            var timeKeeper = new FakeTimeKeeper("2018", "09");
            Assert.IsTrue(sut.ScoredLastGame(timeKeeper) );
        }

        [TestMethod]
        public void Player_WhenScoredInLastGameAllowingForBye_ReturnsTrue()
        {
            var sut = new NFLPlayer("MOORDA04");
            var timeKeeper
                = new FakeTimeKeeper("2018", "08"); // game after bye week, tee hee
            Assert.IsTrue(sut.ScoredLastGame(timeKeeper));
        }

        [TestMethod]
        public void Player_WhenScoredInLastTwoAllowingForBye_ReturnsTrue()
        {
            var sut = new NFLPlayer("MOORDA04");
            var timeKeeper = new FakeTimeKeeper("2018", "09");
            Assert.IsTrue(sut.ScoredLastTwo(timeKeeper));
        }

        [TestMethod]
        public void PlayerScoredTwoWeeksAgo_WhenInWeek1_ReturnsFalse()
        {
            var sut = new NFLPlayer("MOORDA04");
            var timeKeeper = new FakeTimeKeeper("2018", "01");
            Assert.IsFalse(sut.ScoredLastTwo(timeKeeper));
        }

        [TestMethod]
        public void PlayerScoredLastWeek_WhenInWeek1_ReturnsFalse()
        {
            var sut = new NFLPlayer("MOORDA04");
            var timeKeeper = new FakeTimeKeeper("2018", "01");
            Assert.IsFalse(sut.ScoredLastGame(timeKeeper));
        }

        [TestMethod]
        public void PlayerScoredTwoWeeksAgo_WhenInWeek2_ReturnsFalse()
        {
            var sut = new NFLPlayer("MOORDA04");
            var timeKeeper = new FakeTimeKeeper("2018", "02");
            Assert.IsFalse(sut.ScoredLastTwo(timeKeeper));
        }
    }
}
