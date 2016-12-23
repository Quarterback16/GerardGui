using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class TeReportTests
   {
      [TestMethod]
      public void TestTeReport()
      {
         var sut = new TeReport( new FakeTimeKeeper( season: "2016" ) );
         sut.RenderAsHtml();
         Assert.IsTrue( File.Exists( sut.FileOut ) );
         Console.WriteLine( "{0} created.", sut.FileOut );
      }
   }
}
