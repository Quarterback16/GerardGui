using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class BalanceReportTests
   {
      [TestMethod]
      public void TestTimetoDoBalanceReport()
      {
         var tk = new FakeTimeKeeper( "2015" );
         Assert.IsTrue( tk.PreviousSeason().Equals("2014"));
         var sut = new BalanceReportJob( tk );
         string whyNot;
         Assert.IsTrue(sut.IsTimeTodo(out whyNot));
         Console.WriteLine(whyNot);
      }

      [TestMethod]
      public void TestDoBalanceReportJob()
      {
         var tk = new FakeTimeKeeper("2015");
         var sut = new BalanceReportJob(tk);
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
         Console.WriteLine(outcome);
      }

      [TestMethod]
      public void TestDoBalanceReportJobLastFiveYears()
      {
         var tk = new FakeTimeKeeper("2014");
         while (Int32.Parse( tk.PreviousSeason() ) > 2009 )
         {
            var sut = new BalanceReportJob(tk);
            var outcome = sut.DoJob();
            Assert.IsFalse(string.IsNullOrEmpty(outcome));
            Console.WriteLine(outcome);
            tk.Season = tk.PreviousSeason();
         }
      }

      [TestMethod]
      public void TestDoBalanceReportJob2012()
      {
         var tk = new FakeTimeKeeper("2013");
         var sut = new BalanceReportJob(tk);
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
         Console.WriteLine(outcome);
      }

   }
}
