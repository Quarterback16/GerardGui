using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.Data;

namespace Gerard.Tests
{
	[TestClass]
	public class DataLibrarianTests
	{
		[TestMethod]
		public void TestGetPlayer()
		{
			var results = Utility.TflWs.GetPlayer( "MONTJO01" );
			Assert.IsTrue( results.Tables[ 0 ].Rows.Count == 1 );
		}

		[TestMethod]
		public void TestGetPlayerName()
		{
			var results = Utility.TflWs.GetPlayerName( "MONTJO01" );
			Assert.IsTrue( results.Equals( "J Montana" ) );
		}

		[TestMethod]
		public void TestGetTeamPlayers()
		{
			var results = Utility.TflWs.GetTeamPlayers( "SF", "2" );
			Assert.IsTrue( results.Tables[ 0 ].Rows.Count > 0 );
		}

		[TestMethod]
		public void TestGetRegularSeason()
		{
			var results = Utility.TflWs.GetRegularSeason( "AF", "2016" );
			Assert.IsTrue( results.Tables[ 0 ].Rows.Count.Equals( 16 ) );
		}

		[TestMethod]
		public void TestGetSeason()
		{
			var results = Utility.TflWs.GetSeason( "AF", "2016" );
			Assert.IsTrue( results.Tables[ 0 ].Rows.Count.Equals( 19 ) );
		}

		[TestMethod]
		public void TestGetPreviousSeason()
		{
			var ds = Utility.TflWs.GetLastRegularSeasonGames(
				"AF", Constants.K_GAMES_IN_REGULAR_SEASON, new DateTime(2017,8,8) );
			var dt = ds.Tables[ "SCHED" ];
			var nGameCount = 0;
			foreach ( DataRow dr in dt.Rows )
			{
				if ( dr.RowState == DataRowState.Deleted ) continue;
				var g = new NFLGame( dr );
				Assert.IsTrue( g.IsRegularSeasonGame() );
				Assert.IsTrue( g.Season.Equals("2016") );
				nGameCount++;
			}
			Assert.AreEqual( expected: 16, actual: nGameCount );
		}
	}
}