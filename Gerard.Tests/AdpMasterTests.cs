using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib.Helpers;

namespace Gerard.Tests
{
    [TestClass]
    public class AdpMasterTests
    {
        private AdpMaster _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new AdpMaster(".\\Output\\XML\\adp.csv");
        }

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void AdpMaster_LoadWithBadPath_ThrowsException()
        {
            _sut.Load(
                ".\\Output\\2019\\Starters\\csv\\Starters.csv");
        }

        [TestMethod]
        public void AdpMasterLoad_WithGoodPath_ReturnsNumberOfPlayers()
        {
            var result = _sut.Load();
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void AdpMaster_InstantiatesOk()
        {
            Assert.IsNotNull(_sut);
        }

        [TestMethod]
        public void AdpMasterGet_ForUnknownPlayer_ReturnsEmptyString()
        {
            _sut.Load();
            var adp = _sut.GetAdp("Steve Colonna");
            Assert.AreEqual(string.Empty, adp);
        }

        [TestMethod]
        public void AdpMasterGet_WithoutLoad_PerformsImplicitLoad()
        {
            var adp = _sut.GetAdp("Travis Kelce");
            Assert.AreEqual("2.07", adp);
        }

        [TestMethod]
        public void AdpMasterGet_ForSaquon_Returns102()
        {
            _sut.Load();
            var adp = _sut.GetAdp("Saquon Barkley");
            Assert.AreEqual("1.02",adp);
        }

        [TestMethod]
        public void AdpMasterGet_ForMahomes_Returns210()
        {
            _sut.Load();
            var adp = _sut.GetAdp("Patrick Mahomes");
            Assert.AreEqual("2.10", adp);
        }

        [TestMethod]
        public void AdpMasterGet_ForBeckham_Returns203()
        {
            _sut.Load();
            var adp = _sut.GetAdp("Odel Beckham");
            Assert.AreEqual("2.03", adp);
        }
    }
}
