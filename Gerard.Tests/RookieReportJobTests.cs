using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.ReportGenerators;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class RookieReportJobTests
   {
      [TestMethod]
      public void TestRookiesJob()
      {
         var sut = new RookiesJob(new FakeTimeKeeper("2016"));
         sut.Execute();
         var run = sut.Report.LastRun;
         Assert.IsTrue(run.Date.Equals(DateTime.Now.Date));
      }

      [TestMethod]
      public void TestRookieRBsJobForG1()
      {
         var sut = new RookieReportGenerator();
         var cfg = new RookieConfig
         {
            Category = Constants.K_RUNNINGBACK_CAT,
            Position = "RB"
         };
         var result = sut.GenerateRookieReport(cfg, "G1","2016");

         Assert.IsFalse( string.IsNullOrEmpty( result ) );
         
      }
   }
}