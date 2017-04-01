using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib.TeamReports;
using System;
using System.IO;

namespace Gerard.Tests
{
	[TestClass]
	public class TopDogReportTests
	{
		[TestMethod]
		public void TestTopDogReport()
		{
			var sut = new TopDogReport(	new FakeTimeKeeper( season: "2016", week: "17" ));
			sut.RenderAsHtml();
			Assert.IsTrue( File.Exists( sut.FileOut ) );
			Console.WriteLine( "{0} created.", sut.FileOut );
		}

		[TestMethod]
		public void TestTopDogReportAll()
		{
			for ( int w = 1; w < 18; w++ )
			{
				var sut = new TopDogReport( new FakeTimeKeeper( season: "2016", week: $"{w:0#}" ) );
				sut.RenderAsHtml();
				Assert.IsTrue( File.Exists( sut.FileOut ) );
				Console.WriteLine( "{0} created.", sut.FileOut );
			}

		}
	}
}
