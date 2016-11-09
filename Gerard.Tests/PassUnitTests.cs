using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class PassUnitTests
   {
      [TestMethod]
      public void TestLoad()
      {
         var team = new NflTeam( "PS" );
         var ru = team.LoadPassUnit();
         Console.WriteLine( "   >>> Pass unit loaded {0} receivers; Ace receiver {1}",
            team.PassUnit.Receivers.Count, team.PassUnit.AceReceiver );
         Assert.IsTrue( team.PassUnit.Receivers.Count < 50 );
      }

      [TestMethod]
      public void TestDoubleLoad()
      {
         var team = new NflTeam( "PS" );
         var pu = team.LoadPassUnit();
         Console.WriteLine( "   >>> Pass unit loaded {0} receivers; Ace receiver {1}",
            team.PassUnit.Receivers.Count, team.PassUnit.AceReceiver );
         var count1 = pu.Count;
         var pu2 = team.LoadPassUnit();
         var count2 = pu2.Count;
         Assert.IsTrue( count1 == count2 );
      }
   }
}
