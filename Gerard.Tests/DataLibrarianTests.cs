using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerard.Tests
{
   [TestClass]
   public class DataLibrarianTests
   {
      [TestMethod]
      public void TestGetPlayer() 
      {
         var results = Utility.TflWs.GetPlayer("MONTJO01");
         Assert.IsTrue( results.Tables[0].Rows.Count == 1 );
      }

      [TestMethod]
      public void TestGetTeamPlayers()
      {
         var results = Utility.TflWs.GetTeamPlayers( "SF", "2" );
         Assert.IsTrue( results.Tables[ 0 ].Rows.Count > 0 );
      }
   }
}
