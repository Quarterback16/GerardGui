using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
	[TestClass]
	public class PositionReportTests
	{
		[TestMethod]
		public void TestTeReport()
		{
			var options = new PositionReportOptions
			{
				Topic = "Tight End",
				PositionAbbr = "TE",
				PosDelegate = IsTe,
				PositionCategory = Constants.K_RECEIVER_CAT
			};
			var sut = new PositionReport(
				new FakeTimeKeeper( season: "2017", week: "05" ),
				options );
			sut.RenderAsHtml();
			Assert.IsTrue( File.Exists( sut.FileOut ) );
			Console.WriteLine( "{0} created.", sut.FileOut );
		}

		public bool IsTe( NFLPlayer p )
		{
			return ( p.PlayerCat == Constants.K_RECEIVER_CAT ) && p.Contains( "TE", p.PlayerPos );
		}

		[TestMethod]
		public void TestRbReport()
		{
			var options = new PositionReportOptions
			{
				Topic = "Running Back",
				PositionAbbr = "RB",
				PosDelegate = IsRb,
				PositionCategory = Constants.K_RUNNINGBACK_CAT
			};
			var sut = new PositionReport(
				new FakeTimeKeeper( season: "2016" ),
				options );
			sut.RenderAsHtml();
			Assert.IsTrue( File.Exists( sut.FileOut ) );
			Console.WriteLine( "{0} created.", sut.FileOut );
		}

		[TestMethod]
		public void TestWrReport()
		{
			var options = new PositionReportOptions
			{
				Topic = "Wide Receiver",
				PositionAbbr = "WR",
				PosDelegate = IsWr,
				PositionCategory = Constants.K_RECEIVER_CAT
			};
			var sut = new PositionReport(
			new FakeTimeKeeper( season: "2016" ),
			options );
			sut.RenderAsHtml();
			Assert.IsTrue( File.Exists( sut.FileOut ) );
			Console.WriteLine( "{0} created.", sut.FileOut );
		}

		[TestMethod]
		public void TestQbReport()
		{
			var options = new PositionReportOptions
			{
				Topic = "Quarterback",
				PositionAbbr = "QB",
				PosDelegate = IsQb,
				PositionCategory = Constants.K_QUARTERBACK_CAT
			};
			var sut = new PositionReport(
				new TimeKeeper( null ),
				options );
			sut.RenderAsHtml();
			Assert.IsTrue( File.Exists( sut.FileOut ) );
			Console.WriteLine( "{0} created.", sut.FileOut );
		}

		[TestMethod]
		public void TestPkReport()
		{
			var options = new PositionReportOptions
			{
				Topic = "Kicker",
				PositionAbbr = "PK",
				PosDelegate = IsPk,
				PositionCategory = Constants.K_KICKER_CAT
			};
			var sut = new PositionReport(
				new FakeTimeKeeper( season: "2016" ),
				options );
			sut.RenderAsHtml();
			Assert.IsTrue( File.Exists( sut.FileOut ) );
			Console.WriteLine( "{0} created.", sut.FileOut );
		}

		public bool IsWr( NFLPlayer p )
		{
			return ( p.PlayerCat == Constants.K_RECEIVER_CAT ) && p.Contains( "WR", p.PlayerPos );
		}

		public bool IsRb( NFLPlayer p )
		{
			return ( p.PlayerCat == Constants.K_RUNNINGBACK_CAT ) && p.Contains( "RB", p.PlayerPos );
		}

		public bool IsQb( NFLPlayer p )
		{
			return ( p.PlayerCat == Constants.K_QUARTERBACK_CAT ) && p.Contains( "QB", p.PlayerPos );
		}

		public bool IsPk( NFLPlayer p )
		{
			return ( p.PlayerCat == Constants.K_KICKER_CAT );
		}
	}
}