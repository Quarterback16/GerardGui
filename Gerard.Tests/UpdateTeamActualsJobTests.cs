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
      }

      [TestMethod]
      public void TestUpdatePreviousActualsJob()
      {
         var sut = new UpdateTeamActualsJob( new FakeTimeKeeper( season:"2016", week:"05" ) );
         sut.DoJob();
         var run = sut.Report.LastRun;
         Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
      }
   }
}
