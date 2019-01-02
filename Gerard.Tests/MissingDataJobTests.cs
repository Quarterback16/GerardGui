using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gerard.Tests
{
    [TestClass]
    public class MissingDataJobTests
    {
        [TestMethod]
        public void TestTimetoDoMissingDataJob()
        {
            //  Fake historian garantees job will run always
            var sut = new MissingDataJob(
                new FakeTimeKeeper(
                    season: "2018",
                    week: "17"),
                new FakeHistorian());
            Assert.IsTrue(sut.IsTimeTodo(out string whyNot));
            Console.WriteLine(whyNot);
        }

        [TestMethod]
        public void TestMissingDataJob()
        {
            var sut = new MissingDataJob(
                new FakeTimeKeeper(
                    season: "2018",
                    week: "17"),
                new FakeHistorian());
            var outcome = sut.DoJob();
            Assert.IsFalse(
                string.IsNullOrEmpty(outcome));
        }
    }
}
