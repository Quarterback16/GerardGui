using Butler.Models;
using Gerard.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
    [TestClass]
    public class LoadLineJobTests
    {
        [TestMethod]
        public void TestDoLoadLineJob()
        {
            var sut = new LoadLineJob(
                new TimeKeeper(
                    clock: null),
                new FakeLineMaster());

            var outcome = sut.DoJob();
            Assert.IsFalse(
                string.IsNullOrEmpty(outcome));
            System.Console.WriteLine(outcome);
        }
    }
}
