using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
    [TestClass]
    public class TeamCardTests
    {
        [TestMethod]
        public void TestTeamCardsJob()
        {
            var sut = new TeamCardsJob(new TimeKeeper(null));
            sut.Execute();
            var run = sut.Report.LastRun;
            Assert.IsTrue(run.Date.Equals(DateTime.Now.Date));
        }

        [TestMethod]
        public void TestTeamCardSf()  // 2016-03-23  5 min
        {
            var t = Masters.Tm.GetTeam("2016", "SF");
            var fileOut = t.RenderTeamCard();
            Assert.IsTrue(File.Exists(fileOut));
        }
    }
}