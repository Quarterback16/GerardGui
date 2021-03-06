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
				var sut = new DepthChartReport(
                    new FakeTimeKeeper( season: theSeason ),
                    t.TeamCode );
				sut.Execute();
				if ( sut.HasIntegrityError() )
				{
					isError = true;
					sut.DumpErrors();
					Utility.Announce(
                        $"   Need to fix Depth Chart {t.Name}" );
				}
				t.LoadRushUnit();
				if ( t.RunUnit.HasIntegrityError() )
				{
					isError = true;
					t.RunUnit.DumpUnit();
					t.RunUnit.DumpErrors();
					Utility.Announce( $"   Need to fix  Rushing Unit {t.Name}" );
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
		public void TestDepthChartExecutionParticularTeam()
		{
			const string teamCode = "KC";
			var t = new NflTeam( teamCode );
			var sut = new DepthChartReport(
                new FakeTimeKeeper( season: "2017" ),
                teamCode );
			sut.Execute();
			var isError = false;
			if ( sut.HasIntegrityError() )
			{
				isError = true;
				sut.DumpErrors();
				Utility.Announce( $"   Need to fix Depth Chart {t.Name}" );
			}
			t.LoadRushUnit();
			if ( t.RunUnit.HasIntegrityError() )
			{
				isError = true;
				t.RunUnit.DumpUnit();
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
			var sut = new DepthChartReport(
                new FakeTimeKeeper(
                    season: "2017" ) );
			Assert.IsNotNull( sut );
		}

		[TestMethod]
		public void TestDepthChartLoadsStarters()
		{
			var sut = new DepthChartReport(
                new FakeTimeKeeper( season: "2016" ), "SF" )
			{
				LeagueInFocus = "YH"
			};
			sut.Execute();
			Assert.IsTrue( sut.PlayerCount > 0 );
		}

		[TestMethod]
		public void TestDepthChartFor49ers()
		{
			var sut = new DepthChartReport(
                new FakeTimeKeeper( season: "2020" ),
                "SF" )
			{
				LeagueInFocus = "G1"
			};
			sut.Execute();
			Assert.IsTrue( sut.PlayerCount > 0 );
		}

		[TestMethod]
		public void TestRatingRetrieval()
		{
			var sut = new UnitRatingsService2();

			var results = sut.GetUnitRatingsFor("SF",new DateTime( 2017, 8, 4 ));
			Assert.AreEqual( expected: "DDDDDD", actual: results );
		}

		[TestMethod]
		public void TestDepthChartResultOutputSf()
		{
			var sut = new NFLGame( "2017:01-M" );
			var output = sut.ResultFor( "SF", abbreviate: true );
			Assert.AreEqual( "|   SF v CP   ", output );
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

		[TestMethod]
		public void TestGBRunners()
		{
			var sut = new NflTeam( "GB" );
			var res = sut.LoadRushUnit();
			Assert.IsTrue( res.Count > 0 );
		}

		[TestMethod]
		public void TestGetPlayers()
		{
			var sut = new DepthChartReport(
                timekeeper: new FakeTimeKeeper( season: "2017" ),
                teamCode: "TT" );
			var res = sut.GetPlayers("W2");
			Assert.IsTrue( res.Count == 1 );
		}

        [TestMethod]
        public void TestLeagueInFocus()
        {
            var sut = new DepthChartReport(
                timekeeper: new FakeTimeKeeper(
                    theDate: new DateTime(2018,7,25)),
                teamCode: "NE");
            Assert.AreEqual(
                expected: Constants.K_LEAGUE_Gridstats_NFL1,
                actual: sut.LeagueToFocusOn());
        }

        [TestMethod]
        public void TestRunUnitDump()
        {
            var sut = new NflTeam("CI");
            sut.LoadRushUnit();
            sut.RunUnit.DumpUnit();
        }
    }
}