using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using Butler.Models;

namespace Gerard.Tests
{
   [TestClass]
   public class YahooXmlTests
   {
		[TestMethod]
		public void TestDoYahooXmlJob()  //  9 mins 2015-09-20
		{
			var sut = new YahooXmlJob(new TimeKeeper());
			var outcome = sut.DoJob();
			Assert.IsFalse(string.IsNullOrEmpty(outcome));
		}

      [TestMethod]
      public void TestTimetoDo_YahooXmlJob()
      {
         var sut = new YahooXmlJob(new TimeKeeper());
         string whyNot;
         Assert.IsFalse(sut.IsTimeTodo(out whyNot));
         Console.WriteLine(whyNot);
      }

      [TestMethod]
      public void TestGenerateYahooXml()
      {
         var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
         m.Calculate( Utility.CurrentSeason(), "08" );
         m.Dump2Xml();
      }

      [TestMethod]
      public void TestLineupLoad()
      {
         var LineupDs = Utility.TflWs.GetLineup( "NO", "2015", 8 );
         var lineup = new Lineup(LineupDs);
         Utility.Announce(string.Format("NFLGame.LoadPlayers {0} players in lineup", lineup.PlayerList.Count ) );
         lineup.DumpLineup();
         Assert.IsTrue(lineup.PlayerList.Count > 0);
      }

      [TestMethod]
      public void TestYahooScorerOnAPlayer()
      {
         var w = new NFLWeek( 2014, 8 );
         var scorer = new YahooScorer( w );
         var plyr = new NFLPlayer( "LACYED01" );
         var score = scorer.RatePlayer( plyr, w );
         Assert.IsTrue( score == 17.0M );
         var gameKey = w.GameCodeFor( plyr.TeamCode );
         var msg = plyr.MetricsOut(gameKey);
         Console.WriteLine( msg );
      }

      [TestMethod]
      public void TestDoFullYahooXmlJob()  //   fails at 104 mins
      {
         var sut = new YahooXmlFullJob(new TimeKeeper());
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
      }
   }
}
