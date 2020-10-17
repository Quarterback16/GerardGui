using Butler.Dto;
using Butler.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gerard.Tests
{
    [TestClass]
    public class LineMasterTests
    {
        private LineMaster _cut;

        [TestInitialize]
        public void TestInitialize()
        {
            _cut = new LineMaster();
        }

        [TestMethod]
        public void LineMaster_CanCallWebApi()
        {
            //  need to have the LineService API running
            //  solution = MySportsFeed.NetCore
            _cut.Load();
        }

        [TestMethod]
        public void LineMaster_CanConvertTeamCodes()
        {
            var result = _cut.TeamFor("SF");
            Assert.AreEqual("SF", result);
        }

        [TestMethod]
        public void LineMaster_CanConvertTeamCodes_Chicago()
        {
            var result = _cut.TeamFor("CHI");
            Assert.AreEqual("CH", result);
        }

        [TestMethod]
        public void LineMaster_CanConvertGameLineDto_To_GameKey()
        {
            var result = _cut.KeyFromGameLineDto(
                "CHI @ SF",
                new DateTime(2020,10,17));
            Assert.AreEqual(
                "2020-10-17:SF",
                result);
        }

        [TestMethod]
        public void LineMaster_CaddAddGameLineToMemory()
        {
            var gameLineDto = new GameLineDto
            {
                Game = "HOU @ TEN",
                Spread = -3.5M,
                Total = 53.5M
            };
            _cut.AddGameLine(
                gameLineDto,
                new DateTime(2020,10,18));
            Assert.IsTrue(
                _cut.Lines.Count.Equals(1));
            foreach (var item in _cut.Lines)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod]
        public void LineMaster_Knows32_TeamCodes()
        {
            var result = _cut.TeamCode.Count;
            Assert.AreEqual(32, result);
        }

        [TestMethod]
        public void LineMaster_SupplyLines()
        {
            var gameDate = new DateTime(2020, 10, 18);
            //var gameLineDto = new GameLineDto
            //{
            //    Game = "HOU @ TEN",
            //    Spread = -3.5M,
            //    Total = 53.5M
            //};
            //_cut.AddGameLine(
            //    gameLineDto,
            //    gameDate);
            var result = _cut.GetLine(
                gameDate,
                "TT");
            Assert.AreEqual(
                3.5M,
                result.Spread);
        }
    }
}
