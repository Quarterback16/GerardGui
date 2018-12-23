using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
    [TestClass]
    public class DataIntegrityCheckerTests
    {
        [TestMethod]
        public void IntegrityChecker_ChecksScores()
        {
            var sut = new DataIntegrityChecker("2018", 0);  // all weeks
            //sut.CheckScores();
            //sut.CheckStats();
            //sut.CheckGames();
            sut.CheckLineups();
        }

        [TestMethod]
        public void IntegrityChecker_JobRuns()
        {
            var sut = new DataIntegrityJob(
                new TimeKeeper(null) );
            sut.DoJob();
        }
    }
}
