using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class PointsAllowedReportTests
   {
      [TestMethod]
      public void TestReport()
      {
         var sut = new PointsAllowedReport(
            new FakeTimeKeeper( season: "2016", week: "17" ) );
         sut.RenderAsHtml();
         Assert.IsTrue( File.Exists( sut.FileOut ) );
         Console.WriteLine( "{0} created.", sut.FileOut );
      }
   }
}
