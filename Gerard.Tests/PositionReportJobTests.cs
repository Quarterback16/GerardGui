using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
    [TestClass]
    public class PositionReportJobTests
    {
        [TestMethod]
        public void TestTimetoDoPositionReports()
        {
            var sut = new PositionReportJob(new TimeKeeper(clock: null));
            Assert.IsFalse(sut.IsTimeTodo(out string whyNot));
            Console.WriteLine(whyNot);
        }
    }
}
