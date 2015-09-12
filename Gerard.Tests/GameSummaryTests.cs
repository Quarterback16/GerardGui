using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class GameSummaryTests
   {

      [TestMethod]
      public void TestGameSummary()
      {
         var game = new NFLGame("2014:01-A");

         var sut = new GameSummary(game);
         sut.Render();
         var fileOut = sut.FileName();

         Assert.IsTrue(File.Exists(fileOut), string.Format("Cannot find {0}", fileOut));
      }

      [TestMethod]
      public void TestSpecialTeamScores()
      {
         var game = new NFLGame("2014:13-N");

         Assert.IsTrue( game.HomeTDs.Equals(0) );
      }

   }
}
