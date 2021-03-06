using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
    [TestClass]
    public class FantasyReportJobTests
    {
        [TestMethod]
        public void TestFantasyReportJob()
        {
            var sut = new FantasyReportJob(new TimeKeeper(null));
            sut.DoJob();
            var run = sut.Report.LastRun;
            Assert.IsTrue(run.Date.Equals(DateTime.Now.Date));
        }

        [TestMethod]
        public void TestTimetoDoFantasyReportReport()
        {
            var sut = new FantasyReportJob(
                new FakeTimeKeeper(season: "2015", week: "10"));
            Assert.IsTrue(sut.IsTimeTodo(out _));
        }
    }
}
