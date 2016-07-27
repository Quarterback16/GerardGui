using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class UnitRatingTests
	{
		[TestMethod]
		public void TestRatingsTeamLoadingNo()
		{
			var teamList = new List<NflTeam>();
			var sut = new UnitRatingsService();
			sut.TallyTeam( teamList, "2013", new DateTime( 2013, 10, 17 ), "NO" );
			var sutTeam = teamList[0];
			Assert.IsTrue( sutTeam.GameList.Count.Equals( 16 ) );
			Assert.IsTrue( sutTeam.TotYdp.Equals( 1957 ), string.Format( "Was expecting {0} but got {1}", 1957, sutTeam.TotYdp ) );
		}

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
		public void TestRatingsRetrieval2()
		{
			var team = new NflTeam("SF");
			var currRatings = team.Ratings;
			const string expectedValue = "EACCBD";
			Assert.IsTrue(currRatings.Equals(expectedValue),
				string.Format("SF team rating should be {1} not {0}", currRatings, expectedValue));
		}

		[TestMethod]
		public void TestLoadPreviousSeasonGmes()
		{
			var team = new NflTeam("SF");
			team.LoadPreviousRegularSeasonGames(team.TeamCode, "2014", new DateTime(2014, 9, 7));
			Assert.IsTrue(team.GameList.Count.Equals(16));
			var game1 = (NFLGame) team.GameList[15];
			Assert.IsTrue(game1.Season.Equals("2013"));
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
		public void TestRatingsTeamLoadingSf()
		{
			var teamList = new List<NflTeam>();
			var sut = new UnitRatingsService();
			sut.TallyTeam(teamList, "2014", new DateTime(2014, 9, 1), "SF");
			var sutTeam = teamList[0];
			Assert.IsTrue(sutTeam.GameList.Count.Equals(16));
			Assert.IsTrue(sutTeam.TotYdp.Equals(1957), string.Format("Was expecting {0} but got {1}", 1957, sutTeam.TotYdp));
		}

		//[TestMethod]
		//public void TestSundays()
		//{
		//	var sut = new UnitRatingsService();
		//	var sundays = sut.SundaysFor("2014") ;
		//	var i = 0;
		//	foreach (var sunday in sundays)
		//	{
		//		i++;
		//		Console.WriteLine("Week {0} is {1}", i, sunday.ToLongDateString());
		//	}
		//	Assert.IsTrue(sundays.Count.Equals(17) );
		//}

		[TestMethod]
		public void TestGetSunday2014()
		{
			var sut = new UnitRatingsService2();
			var theSunday = sut.GetSundayFor(new DateTime(2014, 7, 20));
			Assert.IsTrue(theSunday.Equals(new DateTime(2014,9,7)));
		}

      [TestMethod]
      public void TestGetSunday2016()
      {
         var sut = new UnitRatingsService2();
         var theSunday = sut.GetSundayFor( new DateTime( 2016, 7, 27 ) );
         Assert.IsTrue( theSunday.Equals( new DateTime( 2016, 9, 11 ) ) );
      }

      [TestMethod]
		public void TestNewRatingsRetrieval2()
		{
			var sut = new UnitRatingsService2();
			var currRatings = sut.GetUnitRatingsFor( "SF", new DateTime(2014,7,20));
			const string expectedValue = "EACCBD";
			Assert.IsTrue(currRatings.Equals(expectedValue),
				string.Format("SF team rating should be {1} not {0}", currRatings, expectedValue));
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

	}
}
