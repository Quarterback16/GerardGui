using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
    [TestClass]
    public class EspnScorerTests
    {
        private EspnScorer _cut;
    
        [TestInitialize]
        public void TestInitialize()
        {
            var master = new YahooMaster(
                "Yahoo",
                "YahooOutput.xml");
            var week = new NFLWeek("2020", "01");
            _cut = new EspnScorer(
                week)
            {
                Master = master,
                AnnounceIt = true
            };
        }

        [TestMethod]
        public void Scorer_CanScoreMahomes()
        {
            var week = new NFLWeek("2020", "01");
            var p = new NFLPlayer("MAHOPA01");
            var result = _cut.RatePlayer(
                p,
                week);
            Assert.AreEqual(20.44M,result);
        }

        [TestCleanup]
        public void TearDown()
        {
        }
    }
}
