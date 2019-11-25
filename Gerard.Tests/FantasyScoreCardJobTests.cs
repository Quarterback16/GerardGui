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

        [TestMethod]
        public void TestTimeToDoFantasyReportJob()
        {
            var sut = new FantasyScoreCardJob(
                new FakeTimeKeeper(
                    season: "2019",
                    week: "11",
                    theDate: new DateTime(2019, 11, 18, 6, 0, 0)));
            var result = sut.IsTimeTodo(out string whyNot);
            Console.WriteLine(whyNot);
            Assert.IsFalse(result,"Should not be time in the Monday am");
        }
    }
}
