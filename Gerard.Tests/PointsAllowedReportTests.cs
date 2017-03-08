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
         for ( int i = 11; i < 18; i++ )
         {
            var sut = new PointsAllowedReport(
               new FakeTimeKeeper( season: "2016", week: $"{i:0#}" ) );
            sut.RenderAsHtml();
            Assert.IsTrue( File.Exists( sut.FileOut ) );
            Console.WriteLine( "{0} created.", sut.FileOut );
         }
      }
   }
}
