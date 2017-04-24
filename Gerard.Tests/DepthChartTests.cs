using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class DepthChartTests
	{
		[TestMethod]
		public void TestDoDepthChartJob()  //  2015-09-10 5 mins, 20 min debug
		{
			var sut = new DepthChartJob( new TimeKeeper( null ) );
			var outcome = sut.DoJob();
			Console.WriteLine( "outcome={0}", outcome );
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestTimetoDoDepthCharts()
		{
			var sut = new DepthChartJob( new TimeKeeper( null ) );
			Assert.IsFalse( sut.IsTimeTodo( out string whyNot ) );
			Console.WriteLine( whyNot );
		}

		[TestMethod]
		public void TestAllDepthCharts()
		{
			const string theSeason = "2015";
			var errors = 0;
			var errorTeams = string.Empty;
			var s = new NflSeason( theSeason, true );
			foreach ( var t in s.TeamList )
			{
				var isError = false;
				var sut = new DepthChartReport( new FakeTimeKeeper( season: theSeason ), t.TeamCode );
				sut.Execute();
				if ( sut.HasIntegrityError() )
				{
					isError = true;
					sut.DumpErrors();
					Utility.Announce( string.Format( "   Need to fix Depth Chart {0}", t.Name ) );
				}
				t.LoadRushUnit();
				if ( t.RushUnit.HasIntegrityError() )
				{
					isError = true;
					t.RushUnit.DumpUnit();
					t.RushUnit.DumpErrors();
					Utility.Announce( string.Format( "   Need to fix  Rushing Unit {0}", t.Name ) );
				}
				t.LoadPassUnit();
				if ( t.PassUnit.HasIntegrityError() )
				{
					isError = true;
					t.PassUnit.DumpUnit();
					t.PassUnit.DumpErrors();
					Utility.Announce( string.Format( "   Need to fix  Passing Unit {0}", t.Name ) );
				}
				if ( isError )
				{
					errorTeams += t.TeamCode + ",";
					errors++;
				}
			}
			Utility.Announce( "   -------------------------------------------------" );
			Utility.Announce( string.Format( "   There are {0} broken teams - {1}", errors, errorTeams ) );
			Utility.Announce( "   -------------------------------------------------" );
			Assert.AreEqual( 0, errors );
		}

		[TestMethod]
		public void TestDepthChartExecution()
		{
			const string teamCode = "NE";
			var t = new NflTeam( teamCode );
			var sut = new DepthChartReport( new FakeTimeKeeper( season: "2015" ), teamCode );
			sut.Execute();
			var isError = false;
			if ( sut.HasIntegrityError() )
			{
				isError = true;
				sut.DumpErrors();
				Utility.Announce( string.Format( "   Need to fix Depth Chart {0}", t.Name ) );
			}
			t.LoadRushUnit();
			if ( t.RushUnit.HasIntegrityError() )
			{
				isError = true;
				t.RushUnit.DumpUnit();
				Utility.Announce( string.Format( "   Need to fix  Rushing Unit {0}", t.Name ) );
			}
			t.LoadPassUnit();
			if ( t.PassUnit.HasIntegrityError() )
			{
				isError = true;
				t.PassUnit.DumpUnit();
				Utility.Announce( string.Format( "   Need to fix  Passing Unit {0}", t.Name ) );
			}
			Assert.IsFalse( isError );
		}

		[TestMethod]
		public void TestDepthChartConstructor()
		{
			var sut = new DepthChartReport( new FakeTimeKeeper( season: "2017" ) );
			Assert.IsNotNull( sut );
		}

		[TestMethod]
		public void TestDepthChartLoadsStarters()
		{
			var sut = new DepthChartReport( new FakeTimeKeeper( season: "2016" ), "SF" )
			{
				LeagueInFocus = "YH"
			};
			sut.Execute();
			Assert.IsTrue( sut.PlayerCount > 0 );
		}

		[TestMethod]
		public void TestDepthChartResultOutput()
		{
			var sut = new NFLGame( "2016:01-P" );
			var output = sut.ResultFor( "SF", abbreviate: true );
			Assert.AreEqual( "|W v SL 28-0", output );
		}

		[TestMethod]
		public void TestDepthChartResultOutputPreSeason()
		{
			var sut = new NFLGame( "2017:01-M" );
			var output = sut.ResultFor( "SF", abbreviate: true );
			Assert.AreEqual( "|  SF v CP  ", output );
		}

		[TestMethod]
		public void TestMoranNorrisIsNotStarter()
		{
			var role = "?";
			var sut = new DepthChartReport( new FakeTimeKeeper( season: "2016" ), "SF" );
			sut.Execute();
			foreach ( var p in sut.NflTeam.PlayerList )
			{
				var player = ( NFLPlayer ) p;
				if ( p.ToString() != "Moran Norris" ) continue;
				role = player.PlayerRole;
				break;
			}
			Assert.AreNotEqual( "S", role );
		}

		[TestMethod]
		public void TestDepthChartRatingsOut()
		{
			var sut = new NflTeam( "SF" ) { Ratings = "CBEAAB" };
			var spreadRatings = sut.RatingsOut();
			Assert.AreEqual( "C B E - A A B : 39", spreadRatings );
		}

		[TestMethod]
		public void TestDepthChartRatingsOutNj()
		{
			var sut = new NflTeam( "NJ" );
			var spreadRatings = sut.RatingsOut();
			Assert.AreNotEqual( "? A ? - ? ? ?", spreadRatings );
		}

		[TestMethod]
		public void TestSpreadOutRatings()
		{
			var sut = new NflTeam( "SF" ) { Ratings = "ABCDEF" };
			var spreadRatings = sut.SpreadoutRatings();
			Assert.AreEqual( "A B C - D E F", spreadRatings );
		}

		[TestMethod]
		public void TestSpreadOutRatingsError()
		{
			var sut = new NflTeam( "SF" ) { Ratings = "ABC" };
			var spreadRatings = sut.SpreadoutRatings();
			Assert.AreEqual( "??????", spreadRatings );
		}

		[TestMethod]
		public void TestPoRatings()
		{
			var sut = new NflTeam( "SF" ) { Ratings = "ABCDEF" };
			var rating = sut.PoRating();
			Assert.AreEqual( "A", rating );
		}

		[TestMethod]
		public void TestOldDate()
		{
			var sTo = DateTime.Parse( "14/12/2013" ).ToString( "dd/MM/yyyy" );
			var dTo = sTo == "30/12/1899" ? DateTime.Now : DateTime.Parse( sTo );
			Assert.AreEqual( dTo, new DateTime( 14, 12, 2014 ) );
		}

		[TestMethod]
		public void TestWeekHasPassed()
		{
			var sut = new NFLWeek( "2014", 1 );
			Assert.IsFalse( sut.HasPassed() );
		}
	}
}