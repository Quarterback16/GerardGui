using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class UnitRatingTests
	{
		[TestMethod]
		public void TestRatingsRetrieval()
		{
			var team = new NflTeam( "SF" );
			var rr = new UnitRatingsService();
			var currRatings = rr.GetUnitRatingsFor( team, new DateTime( 2014, 9, 7 ) );  //  first Sunday of the 2014 season
			const string expectedValue = "EACCBD";
			Assert.IsTrue( currRatings.Equals( expectedValue ),
				string.Format( "SF team rating should be {1} not {0}", currRatings, expectedValue ) );
		}

		[TestMethod]
		public void TestLoadPreviousSeasonGmes()
		{
			var team = new NflTeam( "SF" );
			team.LoadPreviousRegularSeasonGames( team.TeamCode, "2014", new DateTime( 2014, 9, 7 ) );
			Assert.IsTrue( team.GameList.Count.Equals( 16 ) );
			var game1 = ( NFLGame ) team.GameList[ 15 ];
			Assert.IsTrue( game1.Season.Equals( "2013" ) );
		}

		[TestMethod]
		public void TestUnitRatingsRetrieval()
		{
			var sut = new UnitRatingsService();
			var bActual = sut.HaveAlreadyRated( new DateTime( 2015, 11, 1 ) );
			Assert.IsTrue( bActual );
		}

		[TestMethod]
		public void TestUnitRatingsStorage()
		{
			Utility.TflWs.SaveUnitRatings( "XXXXXX", new DateTime( 2013, 8, 5 ), "??" );
			Assert.IsTrue( true );
		}

		[TestMethod]
		public void TestGetSunday2014()
		{
			var sut = new UnitRatingsService2();
			var theSunday = sut.GetSundayFor( new DateTime( 2014, 7, 20 ) );
			Assert.IsTrue( theSunday.Equals( new DateTime( 2014, 9, 7 ) ) );
		}

		[TestMethod]
		public void TestGetSunday2016()
		{
			var sut = new UnitRatingsService2();
			var theSunday = sut.GetSundayFor( new DateTime( 2016, 9, 9 ) );
			Assert.IsTrue( theSunday.Equals( new DateTime( 2016, 9, 11 ) ) );
		}

		[TestMethod]
		public void TestNewRatingsRetrieval2_2016()
		{
			var sut = new UnitRatingsService2();
			var currRatings = sut.GetUnitRatingsFor( "CP", new DateTime( 2016, 9, 9 ) );
			const string expectedValue = "DACCBA";
			Assert.IsTrue( currRatings.Equals( expectedValue ),
			   string.Format( "CP team rating should be {1} not {0}", currRatings, expectedValue ) );
		}

		[TestMethod]
		public void TestNewRatingsRetrieval2()
		{
			var sut = new UnitRatingsService2();
			var currRatings = sut.GetUnitRatingsFor( "SF", new DateTime( 2014, 7, 20 ) );
			const string expectedValue = "EACCBD";
			Assert.IsTrue( currRatings.Equals( expectedValue ),
				string.Format( "SF team rating should be {1} not {0}", currRatings, expectedValue ) );
		}

		[TestMethod]
		public void TestNewRatingsRetrievalDB()
		{
			var team = new NflTeam( "DB" );
			var sut = new UnitRatingsService();
			var currRatings = sut.GetUnitRatingsFor( team, new DateTime( 2015, 11, 1 ) );  //  Date must be a Sunday
			const string expectedValue = "CECBAA";
			Assert.IsTrue( currRatings.Equals( expectedValue ),
				string.Format( "{0} team rating should be {2} not {1}", team.TeamCode, currRatings, expectedValue ) );
		}

		[TestMethod]
		public void TestThatTheCurrentSeasonHasAStartDate()
		{
			//  need to remember the task to do this in Season initialisation
			var sut = new UnitRatingsService2();
			var sunday = sut.GetSundayFor( DateTime.Now );   //  current
			Assert.IsTrue( sunday != new DateTime(1,1,1) );
		}
	}
}