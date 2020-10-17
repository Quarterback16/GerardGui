using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System.IO;

namespace Gerard.Tests
{
    [TestClass]
    public class LineupTests
    {
        [TestMethod]
        public void TestLineupSF()
        {
            var team = new NflTeam("SF");
            team.SpitLineups(bPersist: true);
            var fileOut = team.LineupFile();
            Assert.IsTrue(
                File.Exists(fileOut),
                $"Cannot find {fileOut}");
        }

        [TestMethod]
        public void TestFirstGameLineup()
        {
            var game = new NFLGame("2020:01-A");
            var result = game.LoadLineupPlayers(
                "KC");
            Assert.IsTrue(result.Count == 34);
        }
    }
}
