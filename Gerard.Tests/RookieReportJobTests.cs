using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class RookieReportJobTests
   {
      [TestMethod]
      public void TestRookiesJob()
      {
         var sut = new RookiesJob(new FakeTimeKeeper("2015"));
         sut.Execute();
         var run = sut.Report.LastRun;
         Assert.IsTrue(run.Date.Equals(DateTime.Now.Date));
      }
   }
}