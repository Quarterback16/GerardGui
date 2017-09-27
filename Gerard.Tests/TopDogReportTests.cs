using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
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

		[TestMethod]
		public void TopQBDogForHTWeek1_ShouldBeWatson()
		{
			var season = "2017";
			var week = "01";
			var teamcode = "HT";

			//  Savage and Watson shared the QB duties
			//  Savage was 7/13 for 62 yards (2pts)
			//  Watson was 12/23 for 102 yards 1 INT (2pts?)
			//  so NO top dog??
			//  stats come from the Yahoo Xml

			var ds = Utility.TflWs.GameForTeam( season, week, teamcode );
			var sut = new TopDogReport( new FakeTimeKeeper( season: season, week: week ) )
			{
				PositionAbbr = "QB",
				PositionCategory = Constants.K_QUARTERBACK_CAT
			};
			var plyr = sut.TopDog( new NflTeam( teamcode ), week, ds );
		}
	}
}
