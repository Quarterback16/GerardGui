using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using Butler.Models;
using System.IO;

namespace Gerard.Tests
{
    [TestClass]
	public class RoleAssignmentReportTests
	{
        private RoleAssignmentReport _cut;

        [TestInitialize]
        public void TestInitialize()
        {
            _cut = ClassUnderTest();
        }

        private static RoleAssignmentReport ClassUnderTest()
        {
            return new RoleAssignmentReport(
                new FakeTimeKeeper(
                    season: "2019",
                    week: "04" ) );
        }

        [TestMethod]
        public void RenderForSingleTeam_ProducesOutput() 
        {
            _cut.SingleTeam = "PS";
            _cut.RenderAsHtml();
            var outputFile = _cut.OutputFilename();
            Assert.IsTrue(File.Exists(outputFile));
        }

        [TestMethod]
        public void LoadRunners_IncludesJaylenSamuel()
        {
            var team = new NflTeam("PS");
            team.LoadRushUnit();
            //Assert.IsTrue(File.Exists(outputFile));
        }
    }
}
