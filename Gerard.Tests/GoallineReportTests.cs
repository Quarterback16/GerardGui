using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class GoallineReportTests
   {
      [TestMethod]
      public void TestReport()
      {
         var sut = new GoallineReport(
             new FakeTimeKeeper(
                 season: "2019",
                 week: "08" ) );

         sut.Render();

         Assert.IsTrue(
             File.Exists( sut.FileOut ) );
         Console.WriteLine(
             "{0} created.", sut.FileOut );
      }
   }
}
