using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class RushUnitTests
   {
      [TestMethod]
      public void TestLoad()
      {
         var team = new NflTeam("PS");
         var ru = team.LoadRushUnit();
         Console.WriteLine( "   >>> Rush unit loaded {0} rushers; Ace back {1}",
            team.RunUnit.Runners.Count, team.RunUnit.AceBack );
         Assert.IsTrue( team.RunUnit.Runners.Count < 50 && team.RunUnit.Runners.Count > 0 );
      }

      [TestMethod]
      public void TestDoubleLoad()
      {
         var team = new NflTeam( "PS" );
         var ru = team.LoadRushUnit();
         Console.WriteLine( "   >>> Rush unit loaded {0} rushers; Ace back {1}",
            team.RunUnit.Runners.Count, team.RunUnit.AceBack );
         var count1 = ru.Count;
         var ru2 = team.LoadRushUnit();
         var count2 = ru2.Count;
         Assert.IsTrue( count1 == count2 );
      }
   }
}
