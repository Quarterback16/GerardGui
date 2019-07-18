using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.RosterGridReports;
using System.IO;

namespace Gerard.Tests
{
    [TestClass]
    public class PlayerScorecardTests
    {
        private FantasyScorecardReport _sut;

        [TestInitialize]
        public void Setup()
        {
            var playerList = new PlayerLister
            {
                CatCode = "1",
                StartersOnly = true
            };
            
            playerList.Collect(
                catCode: "2",
                sPos: "RB",
                fantasyLeague: Constants.K_LEAGUE_Gridstats_NFL1);

            var pgmDao = new DbfPlayerGameMetricsDao();
            _sut = new FantasyScorecardReport(
                new FakeTimeKeeper(
                    season: "2019",
                    week: "00"),
                pgmDao,
                playerList,
                playerType: "RB");
        }

        [TestMethod]
        public void Sut_GeneratesReportFile()
        {
            _sut.RenderAsHtml();
            Assert.IsTrue(
                File.Exists(_sut.FileOut),
                $"Cannot find {_sut.FileOut}");
        }

    }
}
