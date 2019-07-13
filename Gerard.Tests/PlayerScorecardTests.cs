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
            var pgmDao = new DbfPlayerGameMetricsDao();
            _sut = new FantasyScorecardReport(
                new FakeTimeKeeper(
                    season: "2018",
                    week: "21"),
                pgmDao);
        }

        [TestMethod]
        public void Sut_GeneratesReportFile()
        {
            _sut.PlayerIds.Add("MAHOPA01");
            _sut.RenderAsHtml();
            Assert.IsTrue(
                File.Exists(_sut.FileOut),
                $"Cannot find {_sut.FileOut}");
        }

    }
}
