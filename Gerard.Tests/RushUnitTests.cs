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
            team.RushUnit.Runners.Count, team.RushUnit.AceBack );
         Assert.IsTrue( team.RushUnit.Runners.Count < 50 );
      }

      [TestMethod]
      public void TestDoubleLoad()
      {
         var team = new NflTeam( "PS" );
         var ru = team.LoadRushUnit();
         Console.WriteLine( "   >>> Rush unit loaded {0} rushers; Ace back {1}",
            team.RushUnit.Runners.Count, team.RushUnit.AceBack );
         var count1 = ru.Count;
         var ru2 = team.LoadRushUnit();
         var count2 = ru2.Count;
         Assert.IsTrue( count1 == count2 );
      }
   }
}
