using Butler.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib.Helpers;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class SeasonSchedulerTests
   {
      [TestMethod]
      public void TestOpeningDaySeason2015()
      {
         var sut = new SeasonScheduler();
         Assert.AreEqual( new DateTime(1,1,1), sut.SeasonStarts("2015"));  // schedule not entered as of 2015-03-16 
      }

      [TestMethod]
      public void TestWeekKeyWeek1of2014()
      {
         var sut = new SeasonScheduler();
         Assert.AreEqual("2014:01", sut.WeekKey(new DateTime(2014, 09, 04)));
      }


      [TestMethod]
      public void TestWeekKeyWildCard2014()
      {
         var sut = new SeasonScheduler();
         Assert.AreEqual("2014:18", sut.WeekKey(new DateTime(2015, 01, 01)));
      }

      [TestMethod]
      public void TestWeekKey20150316()
      {
         var sut = new SeasonScheduler();
         Assert.AreEqual("2015:00", sut.WeekKey( new DateTime( 2015,03,16 ) ) );
      }

      [TestMethod]
      public void TestOpeningDaySeason2014ViaWeek()
      {
         var sut = new SeasonScheduler();
         Assert.AreEqual(new DateTime(2014, 9, 4), sut.WeekStarts("2014","01"));
      }

      [TestMethod]
      public void TestWeek1Of2014Ends()
      {
         var sut = new SeasonScheduler();
         Assert.AreEqual(new DateTime( 2014, 9, 8 ), sut.WeekEnds("2014", "01"));
      }


      [TestMethod]
      public void TestClosingDaySeason2014()
      {
         var sut = new SeasonScheduler();
         Assert.AreEqual(new DateTime(2015, 2, 1), sut.SeasonEnds("2014"));
      }

      [TestMethod]
      public void TestClosingDayRegularSeason2014()
      {
         var sut = new SeasonScheduler();
         Assert.AreEqual(new DateTime( 2014, 12, 28), sut.RegularSeasonEnds("2014"));
      }

      [TestMethod]
      public void TestOpeningDaySeason2014()
      {
         var sut = new SeasonScheduler();
         Assert.AreEqual( new DateTime( 2014,9,4), sut.SeasonStarts( "2014" ) );
      }

      [TestMethod]
      public void TestCurrentSeason()
      {
         var sut = new SeasonScheduler();
         Assert.IsFalse( sut.ScheduleAvailable( "2015" ) );
      }

      [TestMethod]
      public void TestSeason2014()
      {
         var sut = new SeasonScheduler();
         Assert.IsTrue(sut.ScheduleAvailable("2014"));
      }
   }
}
