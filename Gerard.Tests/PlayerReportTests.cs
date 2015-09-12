using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class PlayerReportTests
   {

      [TestMethod]
      public void TestPlayerReportsJob()
      {
         var sut = new PlayerReportsJob( new TimeKeeper() );
         sut.DoJob();
         var run = sut.Report.LastRun;
         Console.WriteLine( "Last Run : {0}", run );
         Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
      }

      [TestMethod]
      public void TestTimetoDoPlayerReports()
      {
         var sut = new PlayerReportsJob( new FakeTimeKeeper( isPreSeason:true, isPeakTime:false ) );
         string whyNot;
         var isTime = sut.IsTimeTodo( out whyNot );
         Console.WriteLine( whyNot );
         Assert.IsTrue( isTime );
      }

      [TestMethod]
      public void TestEddieLacy()
      {
         var sut = new NFLPlayer("LACYED01");
         var outcome = sut.PlayerReport();
         Console.WriteLine( "Report generated to {0}", outcome );
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

      [TestMethod]
      public void TestMattForte()
      {
         var sut = new NFLPlayer( "FORTMA01" );
         var outcome = sut.PlayerReport( true );
         Console.WriteLine( "Report generated to {0}", outcome );
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }
   }
}
