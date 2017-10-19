using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class YahooXmlTests
	{
		[TestMethod]
		public void TestDoYahooXmlJob()  //  9 mins 2015-09-20, 23 mins home
		{
			var sut = new YahooXmlJob( new FakeTimeKeeper( season: "2017", week: "06" ) );
			var outcome = sut.DoJob();  // does the previous week
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestTimetoDo_YahooXmlJob()
		{
			var sut = new YahooXmlJob( new TimeKeeper( null ) );
			Assert.IsFalse( sut.IsTimeTodo( out string whyNot ) );
			Console.WriteLine( whyNot );
		}

		[TestMethod]
		public void TestGenerateYahooXmlForEntireSeason()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			m.Calculate( "2017" );
			m.Dump2Xml();
		}

		[TestMethod]
		public void TestGenerateYahooXmlForOneWeek()
		{
			var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
			m.Calculate( season: "2017", week: "01" );
			m.Dump2Xml();
		}

		[TestMethod]
		public void TestLineupLoad()
		{
			var LineupDs = Utility.TflWs.GetLineup( "NO", "2015", 8 );
			var lineup = new Lineup( LineupDs );
			Utility.Announce( $"NFLGame.LoadLineupPlayers {lineup.PlayerList.Count} players in lineup" );
			lineup.DumpLineup();
			Assert.IsTrue( lineup.PlayerList.Count > 0 );
		}

		[TestMethod]
		public void TestYahooScorerOnAPlayer()
		{
			var w = new NFLWeek( 2017, 1 );
			var scorer = new YahooScorer( w );
			var plyr = new NFLPlayer( "WATSDE02" );
			var score = scorer.RatePlayer( plyr, w );
			Assert.IsTrue( score == 2.0M );
			var gameKey = w.GameCodeFor( plyr.TeamCode );
			var msg = plyr.MetricsOut( gameKey );
			Console.WriteLine( msg );
		}

		[TestMethod]
		public void TestYahooScorerOnBeckham()
		{
			var w = new NFLWeek( 2015, 9 );
			var scorer = new YahooScorer( w );
			var plyr = new NFLPlayer( "BECKOD01" );
			var score = scorer.RatePlayer( plyr, w );
			Assert.IsTrue( score > 0.0M );
			var gameKey = w.GameCodeFor( plyr.TeamCode );
			var msg = plyr.MetricsOut( gameKey );
			Console.WriteLine( msg );
		}

		[TestMethod]
		public void TestDoFullYahooXmlJob()  //   fails at 104 mins
		{
			var sut = new YahooXmlFullJob( new TimeKeeper( null ) );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestOneWeek()
		{
			var sut = new NFLGame( "2017:01-L" );
			sut.GenerateYahooOutput();
			Assert.IsTrue( sut.YahooList.Count > 0 );
		}
	}
}