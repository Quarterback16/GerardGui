using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class UpdateTeamActualsJobTests
   {
      [TestMethod]
      public void TestUpdateActualsJob()
      {
         var sut = new UpdateTeamActualsJob( new TimeKeeper( null ) );
         sut.DoJob();
         var run = sut.Report.LastRun;
         Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
         Console.WriteLine( "Check output {0}", sut.Report.OutputFilename() );
      }

      [TestMethod]
      public void TestUpdatePreviousActualsJob()
      {
         var sut = new UpdateTeamActualsJob( new FakeTimeKeeper( season:"2016", week:"05" ) );
         sut.DoJob();
         var run = sut.Report.LastRun;
         Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
      }

      [TestMethod]
      public void TestUpdateGameActuals()
      {
         var g = new NFLGame( "2016:09-A" );
         g.RefreshTotals();
         var g2 = new NFLGame( "2016:09-A" );
         Assert.IsTrue( g2.Spread == g.Spread );
      }
   }
}
