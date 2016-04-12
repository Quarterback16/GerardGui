using Butler;
using Butler.Helpers;
using Butler.Models;
using Helpers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class TeamCardTests
   {

      [TestMethod]
      public void TestTeamCardSf()  // 2016-03-23  5 min
      {
         var t = Masters.Tm.GetTeam("2016", "SF");
         var fileOut = t.RenderTeamCard();
         Assert.IsTrue(File.Exists(fileOut));
      }
   }
}
