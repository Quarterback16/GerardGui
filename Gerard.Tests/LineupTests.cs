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
         var html= team.SpitLineups(bPersist: true);
         var fileOut = team.LineupFile();
         Assert.IsTrue(File.Exists(fileOut), string.Format("Cannot find {0}", fileOut));
      }
   }
}
