using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class RookieReportJobTests
   {
      [TestMethod]
      public void TestRookiesJob()
      {
         var sut = new RookiesJob(new TimeKeeper());
         string whyNot;
         if (sut.IsTimeTodo(out whyNot))
            sut.DoJob();
         var run = sut.Report.LastRun;
         Assert.IsTrue(run.Date.Equals(DateTime.Now.Date));
      }
   }
}