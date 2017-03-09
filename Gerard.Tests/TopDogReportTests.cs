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
	}
}
