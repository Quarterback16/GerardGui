using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class GameProjectionsJobTests
   {
      [TestMethod]
      public void TestDoGameProjectionJob()  //  156 min 2015-08-27, 70 min 2015-12-09
      {
         var sut = new GameProjectionsJob( new TimeKeeper( null ) );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

		[TestMethod]
		public void TestTimeToDoGameProjectionJob()  //    8 sec 2015-08-14 
		{
			var sut = new GameProjectionsJob( new FakeTimeKeeper( season: "2016", week: "18" ) );
			var outcome = sut.IsTimeTodo(out string whyNot);
			Console.WriteLine(whyNot);
			Assert.IsTrue(  outcome );
		}

      [TestMethod]
      public void TestTimeToDoGameProjectionJobPreSchedule()  //    
      {
         var sut = new GameProjectionsJob(new FakeTimeKeeper(season: "2016", week: "00"));
         var outcome = sut.IsTimeTodo(out string whyNot);
         Console.WriteLine(whyNot);
         Assert.IsFalse(outcome);
      }


      [TestMethod]
      public void TestGamePrediction()  //  39 sec 2015-08-10
      {
         var game = new NFLGame( "2015:08-N" );

         var sut = new GameProjection( game ) {AnnounceIt = true};
         sut.Render();
         Assert.IsNotNull( sut );
      }

      [TestMethod]
      public void TestPlayerFantasyProjection()  //  5 sec 2015-08-11 
      {
         var game = new NFLGame("2015:01-A");
         game.GameWeek = new NFLWeek(game.Season, game.Week);
         var scorer = new YahooScorer(game.GameWeek);
         var p = new NFLPlayer("GRONRO01");
         var fpts = scorer.RatePlayer(p, game.GameWeek);
         Assert.AreEqual(fpts,5);
      }

      // select * from PGMETRIC where PLAYERID='TA'UWI01' and GAMECODE = '2014:01-E'

      [TestMethod]
      public void TestQuotesInCommand()
      {
         var game = new NFLGame( "2014:01-E" );

         var pgmDao = new DbfPlayerGameMetricsDao();

         var pgm = pgmDao.Get( "TA'UWI01", game.GameKey() );

         Assert.IsNotNull( pgm );
      }
   }
}