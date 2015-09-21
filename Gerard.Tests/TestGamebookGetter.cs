using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using Helpers;

namespace Gerard.Tests
{
   [TestClass]
   public class TestGamebookGetter
   {
      [TestMethod]
      public void TestCurrentWeek()
      {
         var week = new NFLWeek("2015", "01");
         var sut = new GamebookGetter( new Downloader() );
         var result = sut.DownloadWeek(week);
         Assert.IsTrue(result > 0);
      }

      [TestMethod]
      public void TestCurrentWeekSeed()
      {
         var week = new NFLWeek("2015", "01");
         var sut = new GamebookGetter(new Downloader());
         var result = sut.Seed(week);
         Assert.AreEqual(result, "56503");
      }
   }
}
