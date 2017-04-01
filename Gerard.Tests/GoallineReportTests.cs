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
         var sut = new GoallineReport();
         sut.Season = "2014";
         //sut.Week = "13";
         sut.Render();
         Assert.IsTrue( File.Exists( sut.FileOut ) );
         Console.WriteLine( "{0} created.", sut.FileOut );
      }
   }
}
