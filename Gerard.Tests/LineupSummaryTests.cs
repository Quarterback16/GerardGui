using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.RosterGridReports;
using System;
using System.IO;

namespace Gerard.Tests
{
    [TestClass]
    public class LineupSummaryTests
    {
        [TestMethod]
        public void Constructor_IntantiatesAnObject()
        {
            var sut = new LineupSummary(
                new FakeTimeKeeper( season: "2017", week: "07" ), 8 );
            Assert.IsNotNull( sut );
        }

        [TestMethod]
        public void Summary_GeneratesOutput()
        {
            var sut = new LineupSummary(
                new FakeTimeKeeper( season: "2017", week: "07" ), 7 );
            sut.RenderAsHtml();
            Console.WriteLine( $"{sut.Name} rendered to {sut.FileOut}" );
            Assert.IsTrue( File.Exists( sut.FileOut ) );
        }
    }
}
