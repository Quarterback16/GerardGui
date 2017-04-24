using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class StrengthOfScheduleTests
   {
      [TestMethod]
      public void TestStrengthOfScheduleJob()
      {
         var sut = new StrengthOfScheduleJob( new FakeTimeKeeper(season:"2017") );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

      [TestMethod]
      public void TestTimetoDoStengthOfSchedule()
      {
//         var sut = new StrengthOfScheduleJob( new FakeTimeKeeper() );
         var sut = new StrengthOfScheduleJob( new TimeKeeper(null) );
         string whyNot;
         Assert.IsTrue( sut.IsTimeTodo( out whyNot ) );
         Console.WriteLine( "Final:Reason for not doing>{0}", whyNot );
      }

      [TestMethod]
      public void TestHowStengthOfScheduleDterminesCurrentSeason()
      {
         var sut = new TimeKeeper(null);
         var result = sut.CurrentSeason( new DateTime( 2016, 7, 24 ) );
         Assert.AreEqual( "2016", result );
         Console.WriteLine( "Season>{0}", result );
      }
   }
}