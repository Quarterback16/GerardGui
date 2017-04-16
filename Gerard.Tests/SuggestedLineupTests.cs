using System;
using System.IO;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class SuggestedLineupTests
   {
      [TestMethod]
      public void TestSuggestedLineupsJob()
      {
         var sut = new SuggestedLineupsJob( new FakeTimeKeeper() );
         sut.DoJob();
         var run = sut.Report.LastRun;
         Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
      }

      [TestMethod]
      public void TestSuggestedPerfectChallengeLineupForWeek1()
      {
         var r2 = new SuggestedLineup( Constants.K_LEAGUE_PerfectChallenge,
                                       Constants.KOwnerSteveColonna, "CC",
									   new FakeTimeKeeper( season: "2017", week: "01" ) )
		 {
			 IncludeSpread = true,
			 IncludeRatingModifier = true,
			 IncludeFreeAgents = true
		 };
         r2.Render();
         Assert.IsTrue( File.Exists( r2.FileName() ), string.Format( "Cannot find {0}", r2.FileName() ) );
      }

      [TestMethod]
      public void TestSuggestedPerfectChallengeLineupForWeek1Of2011()
      {
         var r2 = new SuggestedLineup( Constants.K_LEAGUE_PerfectChallenge,
                                       Constants.KOwnerSteveColonna, "CC",
									   new FakeTimeKeeper( season: "2011", week: "01" ) )
		 { IncludeSpread = true, IncludeRatingModifier = true, IncludeFreeAgents = true };
         r2.Render();
         Assert.IsTrue( File.Exists( r2.FileName() ), string.Format( "Cannot find {0}", r2.FileName() ) );
      }

      [TestMethod]
      public void TestSuggestedGridStatsLineupForWeek1()
      {
         var r2 = new SuggestedLineup( Constants.K_LEAGUE_Gridstats_NFL1,
                                       Constants.KOwnerSteveColonna, "CC",
									   new FakeTimeKeeper( season: "2011", week: "01" ) )
		 { IncludeSpread = false, IncludeRatingModifier = false };
         r2.Render();
         Assert.IsTrue( File.Exists( r2.FileName() ), string.Format( "Cannot find {0}", r2.FileName() ) );
      }

      [TestMethod]
      public void TestSuggestedYahooLineupForWeek1()
      {
         //  including free agents will cause a timeout
         var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
                                       Constants.KOwnerSteveColonna, "BB",
									   new FakeTimeKeeper( season: "2011", week: "01" ) )
		 {
            IncludeSpread = true,
            IncludeRatingModifier = true,
            IncludeFreeAgents = false
         };
         r2.Render();
         Assert.IsTrue( File.Exists( r2.FileName() ), string.Format( "Cannot find {0}", r2.FileName() ) );
      }

      private static void TestCamNewtonAveragePreWeek2()
      {
         var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
                                       Constants.KOwnerSteveColonna, "BB",
									   new FakeTimeKeeper( season: "2011", week: "02" ) )
		 {
            IncludeSpread = true,
            IncludeRatingModifier = true,
            IncludeFreeAgents = true
         };

         var p = new NFLPlayer( "NEWTCA01" );

         var pts = r2.AveragePoints( p );
         Assert.AreEqual( string.Format( "{0:0.0}", 9.7 ), string.Format( "{0:0.0}", pts ) );
      }

      private static void TestYahooRankPointsCamNewtonWeek2()
      {
         var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
                                       Constants.KOwnerSteveColonna, "BB",
									   new FakeTimeKeeper( season: "2011", week: "02" ) )
		 {
            IncludeSpread = true,
            IncludeRatingModifier = false,
            IncludeFreeAgents = true
         };

         var p = new NFLPlayer( "NEWTCA01" );
         var g = new NFLGame( "2011:02-I" );
         var t = new NflTeam( "GB" );

         var pts = r2.RankPoints( p, g, t );
         Assert.AreEqual( string.Format( "{0:0.0}", 7.7 ), string.Format( "{0:0.0}", pts ) );
      }

      [TestMethod]
      public void TestSpreadModifiers()
      {
			var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
										  Constants.KOwnerSteveColonna, "BB",
										  new FakeTimeKeeper( season: "2011", week: "02" ) );

		 var m1 = r2.PlayerSpread( 14.0M, isHome: true );
         Assert.AreEqual( 1.4M, m1 );
         var m2 = r2.PlayerSpread( 13.0M, isHome: true );
         Assert.AreEqual( 1.3M, m2 );
         var m3 = r2.PlayerSpread( 9.5M, isHome: true );
         Assert.AreEqual( 1.2M, m3 );
         var m4 = r2.PlayerSpread( 3.0M, isHome: true );
         Assert.AreEqual( 1.1M, m4 );
         var m5 = r2.PlayerSpread( 2.0M, isHome: true );
         Assert.AreEqual( 1.0M, m5 );
         var m6 = r2.PlayerSpread( 14.0M, isHome: false );
         Assert.AreEqual( 0.6M, m6 );
         var m7 = r2.PlayerSpread( 13.0M, isHome: false );
         Assert.AreEqual( 0.7M, m7 );
         var m8 = r2.PlayerSpread( 9.5M, isHome: false );
         Assert.AreEqual( 0.8M, m8 );
         var m9 = r2.PlayerSpread( 3.0M, isHome: false );
         Assert.AreEqual( 0.9M, m9 );
         var m10 = r2.PlayerSpread( 2.0M, isHome: false );
         Assert.AreEqual( 1.0M, m10 );
         var m11 = r2.PlayerSpread( -9.5M, isHome: true );
         Assert.AreEqual( .8M, m11 );
      }

      private static void TestYahooRankPoints()
      {
         var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
                                       Constants.KOwnerSteveColonna, "BB",
									   new FakeTimeKeeper( season: "2011", week: "01" ) )
		 { IncludeSpread = true, IncludeRatingModifier = true, IncludeFreeAgents = true };

         var p = new NFLPlayer( "INGRMA02" );
         var g = new NFLGame( "2011:01-A" );
         var t = new NflTeam( "GB" );

         var pts = r2.RankPoints( p, g, t );
         Assert.AreEqual( -5, pts );
      }

      [TestMethod]
      public void TestYahooRankRatingModifier()
      {
         var r2 = new SuggestedLineup( Constants.K_LEAGUE_Yahoo,
                                       Constants.KOwnerSteveColonna,
                                       "BB",
									   new FakeTimeKeeper( season: "2011", week: "01" ) )
		 {
            IncludeSpread = true,
            IncludeRatingModifier = true,
            IncludeFreeAgents = true
         };

         var modifier = r2.RatingModifier( "A" );  //  an A opponent
         Assert.AreEqual( 0.5M, modifier );
      }
   }
}