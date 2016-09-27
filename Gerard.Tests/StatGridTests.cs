using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler.Models;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class StatGridTests
   {
      [TestMethod]
      public void TestStatGridJob()   // 4 mins
      {
         var sut = new StatGridJob(new TimeKeeper(null));
         sut.DoJob();
         var run = sut.Report.LastRun;
         Assert.IsTrue(run.Date.Equals(DateTime.Now.Date));
      }

      [TestMethod]
      public void TestTimetoDoStatGridJob()
      {
         var sut = new StatGridJob(new FakeTimeKeeper());
         string whyNot;
         Assert.IsFalse(sut.IsTimeTodo(out whyNot));
         Console.WriteLine(whyNot);
      }

   }
}
