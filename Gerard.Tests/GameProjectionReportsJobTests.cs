using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
    [TestClass]
    public class GameProjectionReportsJobTests
    {
        [TestMethod]
        public void TestCurrentGeneratePlayerProjectionsJob()
        {
            var sut = new GameProjectionReportsJob(
                new TimeKeeper(null));
            var resultOut = sut.DoJob();
            Assert.IsTrue(resultOut.Length > 0);
        }

        [TestMethod]
        public void TestTimeToDoGameProjectionJob()  
        {
            var sut = new GameProjectionReportsJob(
                new TimeKeeper(null));
            var outcome = sut.IsTimeTodo(out string whyNot);
            Console.WriteLine(whyNot);
            Assert.IsFalse(outcome);
        }
    }
}