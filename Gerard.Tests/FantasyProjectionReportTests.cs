using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System.IO;
using Butler.Models;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class FantasyProjectionReportTests
   {
      [TestMethod]
      public void TestFantasyProjectionJob()
      {
         var sut = new FantasyProjectionJob(new FakeTimeKeeper());
         sut.DoJob();
         var run = sut.Report.LastRun;
         Assert.IsTrue(run.Date.Equals(DateTime.Now.Date));
      }

      [TestMethod]
      public void TestTimeToDoFantasyProjectionJob()
      {
         var sut = new FantasyProjectionJob(new FakeTimeKeeper());
         string whyNot;
         Assert.IsFalse(sut.IsTimeTodo(out whyNot));
         Console.WriteLine(whyNot);
      }

      [TestMethod]
      
      public void TestRenderAllFantasyProjections()
      {
         //  test a league using yahoo scoring - takes a fair while
         //  out put goes here g:\FileSync\SyncProjects\GerardGui\Gerard.Tests\bin\Debug\Output\2014\Projections\
         var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
         var scorer = new YahooProjectionScorer();  //  Could use a Fake here
         var sut = new FantasyProjectionReport( "2014", "01", dao, scorer ) {League = Constants.K_LEAGUE_Gridstats_NFL1};
         sut.RenderAll();
         var fileOut = sut.FileName();
         Assert.IsTrue( File.Exists(fileOut ) );
      }

      [TestMethod]
      public void TestRenderSingleLeagueRunningbacksProjection()
      {
         //  shorter test 7 mins just on running backs
         var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
         var scorer = new YahooProjectionScorer();  //  Could use a Fake here
         var sut = new FantasyProjectionReport("2014", "14", dao, scorer) {
            League = Constants.K_LEAGUE_Yahoo};
         sut.RenderRunningbacks();
         var fileOut = sut.FileName();
         Assert.IsTrue(File.Exists(fileOut));
      }

      [TestMethod]
      public void TestScoringOnARunningBack()
      {
         var sut = new YahooProjectionScorer();
         var w = new NFLWeek( "2014", "14" );
         var p = new NFLPlayer( "BERNGI01" );
         var gameKey = p.GameKeyFor( "2014", "14" );
         // plyr needs their projections loaded too
         var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
         var pgm = dao.GetPlayerWeek( gameKey, p.PlayerCode );
         p.LoadProjections( pgm );
         var score = sut.RatePlayer( p, w );
         Assert.AreEqual( 12.0M, score );
      }

      [TestMethod]
      public void TestRenderYahooProjection()
      {
         var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
         var scorer = new YahooProjectionScorer();  //  Could use a Fake here
         var sut = new FantasyProjectionReport( "2013", "2", dao, scorer ) {League = Constants.K_LEAGUE_Yahoo};
         var fileOut = sut.FileName();
         Assert.IsTrue(File.Exists(fileOut));
      }

      [TestMethod]
      public void TestRenderTommysProjection()
      {
         var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
         var scorer = new YahooProjectionScorer();  //  Could use a Fake here
         var sut = new FantasyProjectionReport("2013", "4", dao, scorer)
            {
               League = Constants.K_LEAGUE_50_Dollar_Challenge
            };
         sut.RenderAll();
         Assert.IsTrue(File.Exists(sut.FileName()));
      }

      [TestMethod]
      public void TestRenderRantsProjection()
      {
         var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
         var scorer = new YahooProjectionScorer();  //  Could use a Fake here
         var sut = new FantasyProjectionReport("2013", "2", dao, scorer) {League = Constants.K_LEAGUE_Rants_n_Raves};
         sut.RenderAll();
         var fileOut = sut.FileName();
         Assert.IsTrue(File.Exists(fileOut));
      }

      [TestMethod]
      public void TestFileGetsOutputForSF()
      {
         //  small focused test on specific criteria
         var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
         var scorer = new YahooProjectionScorer();  //  Could use a Fake here
         var sut = new FantasyProjectionReport( "2014", "01", dao, scorer ) {TeamFilter = "SF", CategoryFilter = "3"};
         sut.Render();
         var fileOut = sut.FileName();
         Assert.IsTrue(File.Exists(fileOut));
      }

      [TestMethod]
      public void TestFileGetsOutputForWR()
      {
         var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
         var scorer = new YahooProjectionScorer();  //  Could use a Fake here
         var sut = new FantasyProjectionReport( "2014", "1", dao, scorer )
            {
               League = Constants.K_LEAGUE_Gridstats_NFL1,
               CategoryFilter = "3"
            };
         sut.Render();
         var fileOut = sut.FileName();
         Assert.IsTrue(File.Exists(fileOut));
      }

      [TestMethod]
      public void TestFileGetsOutputForBR()
      {
         var dao = new DbfPlayerGameMetricsDao();  //  Could use a Fake here
         var scorer = new YahooProjectionScorer();  //  Could use a Fake here
         var sut = new FantasyProjectionReport( "2014", "1", dao, scorer )
            {
               TeamFilter = "BR",
               League = Constants.K_LEAGUE_Gridstats_NFL1
            };
         sut.Render();
         var fileOut = sut.FileName();
         Assert.IsTrue(File.Exists(fileOut));
      }

   }
}
