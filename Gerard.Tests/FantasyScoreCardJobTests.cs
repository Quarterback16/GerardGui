using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
    [TestClass]
    public class FantasyScoreCardJobTests
    {
        [TestMethod]
        public void TestFantasyReportJob()
        {
            var sut = new FantasyScoreCardJob(new TimeKeeper(null));
            sut.DoJob();
            var run = sut.Report.LastRun;
            Assert.IsTrue(run.Date.Equals(DateTime.Now.Date));
        }
    }
}
