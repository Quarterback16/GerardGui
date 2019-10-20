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
            const string posType = "RB";
            var playerList = new PlayerLister
            {
                StartersOnly = true
            };
            
            playerList.Collect(
                catCode: "2",
                sPos: posType,
                fantasyLeague: Constants.K_LEAGUE_Gridstats_NFL1);

            var pgmDao = new DbfPlayerGameMetricsDao();
            _sut = new FantasyScorecardReport(
                new FakeTimeKeeper(
                    season: "2019",
                    week: "01"),
                pgmDao,
                playerList);
        }

        [TestMethod]
        public void Sut_GeneratesQBReportFile()
        {
            const string posType = "QB";
            var playerList = new PlayerLister
            {
                StartersOnly = true,
                Position = posType
            };

            playerList.Collect(
                catCode: "1",
                sPos: posType,
                fantasyLeague: Constants.K_LEAGUE_Gridstats_NFL1);
            _sut.PlayerList = playerList;
            _sut.RenderAsHtml();
            Assert.IsTrue(
                File.Exists(_sut.FileOut),
                $"Cannot find {_sut.FileOut}");
        }

        [TestMethod]
        public void Sut_GeneratesRBReportFile()
        {
            const string posType = "RB";
            var playerList = new PlayerLister
            {
                StartersOnly = true,
                Position = posType
            };

            playerList.Collect(
                catCode: "2",
                sPos: posType,
                fantasyLeague: Constants.K_LEAGUE_Gridstats_NFL1);
            _sut.PlayerList = playerList;
            _sut.RenderAsHtml();
            Assert.IsTrue(
                File.Exists(_sut.FileOut),
                $"Cannot find {_sut.FileOut}");
        }

        [TestMethod]
        public void Sut_GeneratesWRReportFile()
        {
            const string posType = "WR";
            var playerList = new PlayerLister
            {
                StartersOnly = true,
                Position = posType
            };

            playerList.Collect(
                catCode: "3",
                sPos: posType,
                fantasyLeague: Constants.K_LEAGUE_Gridstats_NFL1);
            _sut.PlayerList = playerList;
            _sut.RenderAsHtml();
            Assert.IsTrue(
                File.Exists(_sut.FileOut),
                $"Cannot find {_sut.FileOut}");
        }

        [TestMethod]
        public void Sut_GeneratesTEReportFile()
        {
            const string posType = "TE";
            var playerList = new PlayerLister
            {
                StartersOnly = true,
                Position = posType
            };

            playerList.Collect(
                catCode: "3",
                sPos: posType,
                fantasyLeague: Constants.K_LEAGUE_Gridstats_NFL1);
            _sut.PlayerList = playerList;
            _sut.RenderAsHtml();
            Assert.IsTrue(
                File.Exists(_sut.FileOut),
                $"Cannot find {_sut.FileOut}");
        }

        [TestMethod]
        public void Sut_GeneratesPKReportFile()
        {
            const string posType = "PK";
            var playerList = new PlayerLister
            {
                StartersOnly = true,
                Position = posType
            };

            playerList.Collect(
                catCode: "4",
                sPos: posType,
                fantasyLeague: Constants.K_LEAGUE_Gridstats_NFL1);
            _sut.PlayerList = playerList;
            _sut.RenderAsHtml();
            Assert.IsTrue(
                File.Exists(_sut.FileOut),
                $"Cannot find {_sut.FileOut}");
        }

        [TestMethod]
        public void Sut_GeneratesAllReportFile()
        {
            _sut.PlayerList = null;
            _sut.RenderAsHtml();
            Assert.IsTrue(
                File.Exists(_sut.FileOut),
                $"Cannot find {_sut.FileOut}");
        }

    }
}
