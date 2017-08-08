using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
	[TestClass]
	public class StatGridTests
	{
		[TestMethod]
		public void TestStatGridJob()   // 4 mins
		{
			var sut = new StatGridJob( new FakeTimeKeeper( season: "2016", week: "18" ) );
			sut.DoJob();
			var run = sut.Report.LastRun;
			Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
		}

		[TestMethod]
		public void TestTimetoDoStatGridJob()
		{
			var sut = new StatGridJob( new FakeTimeKeeper() );
			Assert.IsFalse( sut.IsTimeTodo( out string whyNot ) );
			Console.WriteLine( whyNot );
		}

		[TestMethod]
		public void TestStatGrid()
		{
			var sut = new StatGrid("2016", "YDp");
			sut.ReGenAll();  //  need to count everything up
			sut.Render();
			sut.DumpXml();
			Assert.IsTrue( File.Exists( sut.FileName() ) );
		}

		[TestMethod]
		public void TestStatGridSeasons()
		{
			var theSeason = new NflSeason( "2016" );
			theSeason.LoadRegularWeeksToDate();
			Assert.AreEqual( expected: 17, actual: theSeason.RegularWeeks.Count );
		}

		[TestMethod]
		public void TestStatGridWeeksAllPassed()
		{
			var totalWeeks = 0;
			var theSeason = new NflSeason( "2016" );
			theSeason.LoadRegularWeeks();
			foreach ( var week in theSeason.RegularWeeks )
			{
				if ( !week.HasPassed() )
				{
					Console.WriteLine($"{week.WeekKey()} missing");
					continue;
				}
				totalWeeks++;
			}
			Assert.AreEqual( expected: 17, actual: totalWeeks );
		}

		[TestMethod]
		public void TestStatGridWeekLogic()
		{
			var sut = new NFLWeek( "2016", "14" );
			Assert.IsTrue( sut.HasPassed() );
		}

		[TestMethod]
		public void TestAllGamesHaveBeenPlayed()
		{
			var gamesPlayed = 0;
			var sut = new NFLWeek( "2016", "14" );
			var gList = sut.GameList();
			for ( int i = 0; i < gList.Count; i++ )
			{
				var g = ( NFLGame ) gList[ i ];
				if ( g.Played() )
					gamesPlayed++;
				else
					Console.WriteLine($"Game {g.GameName()}");
			}
			Assert.AreEqual( expected: 16, actual: gamesPlayed );
		}
	}
}