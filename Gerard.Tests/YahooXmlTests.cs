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
      public void TestTimetoDo_YahooXmlJob()
      {
         var sut = new YahooXmlJob(new FakeTimeKeeper());
         string whyNot;
         Assert.IsFalse(sut.IsTimeTodo(out whyNot));
         Console.WriteLine(whyNot);
      }

      [TestMethod]
      public void TestDoYahooXmlJob()
      {
         var sut = new YahooXmlJob( new FakeTimeKeeper() );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

      [TestMethod]
      public void TestGenerateYahooXml()
      {
         var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
         m.Calculate( Utility.CurrentSeason(), "05" );
         m.Dump2Xml();
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
   }
}
