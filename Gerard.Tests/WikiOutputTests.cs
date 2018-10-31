using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System.IO;

namespace Gerard.Tests
{
    [TestClass]
    public class WikiOutputTests
    {
        private WeekPage _sut;

        [TestInitialize]
        public void Setup()
        {
            var week = new NFLWeek(2018, 8);
            _sut = new WeekPage(week);
        }

        [TestMethod]
        public void WeekPage_Outputs_FileName()
        {
            var result = _sut.RenderToWiki();
            Assert.IsTrue(File.Exists(result));
        }
    }
}
