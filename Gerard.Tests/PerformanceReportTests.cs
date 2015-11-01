using System;
using System.Collections.Generic;
using System.IO;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class PerformanceReportTests
   {
      [TestMethod]
      public void TestDoPerformanceReportJob()
      {
         var sut = new PerformanceReportJob( new FakeTimeKeeper("2015","07") );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

      [TestMethod]
      public void TestGoBackOneMonth()
      {
         var sut = new PerformanceReportGenerator();
         var master = new YahooMaster( "Yahoo", "YahooOutput.xml" );
         var theSeason = Int32.Parse( Utility.CurrentSeason() );
         var weekIn = Utility.PreviousWeek();
         var theWeek =
            new NFLWeek( theSeason, weekIn: weekIn, loadGames: false );
         var gs = new EspnScorer( theWeek ) { Master = master, AnnounceIt = true };
         var configs = new List<PerformanceReportConfig>();
         var rpt = new PerformanceReportConfig
         {
            Category = Constants.K_QUARTERBACK_CAT,
            Position = "QB",
            Scorer = gs,
            Week = theWeek,
            WeeksToGoBack = 4
         };
         configs.Add( rpt );
         sut.Configs = configs;  //  overwrite the default configs

         foreach ( var r in sut.Configs )
         {
            sut.GenerateReport( r, Constants.K_LEAGUE_Yahoo );
         }
      }

      [TestMethod]
      public void TestAllPositionsCurrentWeekYahoo()
      {
         var sut = new PerformanceReportGenerator();
         var master = new YahooMaster( "Yahoo", "YahooOutput.xml" );
         var theSeason = Int32.Parse( Utility.CurrentSeason() );
         var weekIn = Utility.PreviousWeek();
         var theWeek =
            new NFLWeek( theSeason, weekIn: weekIn, loadGames: false );
         var gs = new EspnScorer( theWeek ) {Master = master, AnnounceIt = true};
         //var rpt = new PerformanceReportConfig {
         //   Category = Constants.K_QUARTERBACK_CAT, Position = "QB", Scorer = gs, Week = theWeek };

         foreach ( var rpt in sut.Configs )
         {
            sut.GenerateReport( rpt, Constants.K_LEAGUE_Yahoo );
         }
      }

      [TestMethod]
      public void TestGenYahooAllPositionsCurrentWeek()
      {
         var m = new YahooMaster( "Yahoo", "YahooOutput.xml" );
         m.Calculate( "2014", "09" );
         m.Dump2Xml();
         Assert.IsTrue( File.Exists( m.Filename ) );
         //TestAllPositionsBeastModeCurrentWeek();
         TestAllPositionsYahooCurrentWeek();
         //TestAllPositionsRantsCurrentWeek();
      }

      [TestMethod]
      public void TestAllPositionsBeastModeCurrentWeek()
      {
         var fileOut = TestBeastModeQbs();
         fileOut = TestBeastModeRunningBacks();
         fileOut = TestBeastModeTightEnds();
         fileOut = TestBeastModeWideReceivers();
         fileOut = TestBeastModeKickers();
      }

      [TestMethod]
      public void TestAllPositionsYahooCurrentWeek()
      {
         var fileOut = TestYahooQbs();
         //fileOut = TestYahooRunningBacks();
         //fileOut = TestYahooTightEnds();
         //fileOut = TestYahooWideReceivers();
         //fileOut = TestYahooKickers();
      }

      [TestMethod]
      public void TestAllPositionsRantsCurrentWeek()
      {
         var fileOut = TestRantsQbs();
         fileOut = TestRantsRunningBacks();
         fileOut = TestRantsTightEnds();
         fileOut = TestRantsWideReceivers();
         fileOut = TestRantsKickers();
      }

      private string TestRantsRunningBacks()
      {
         var fileOut = WeeklyEspnPerformance( "2", Int32.Parse( Utility.CurrentWeek() ), "RR", "RB" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestRantsKickers()
      {
         var fileOut = WeeklyEspnPerformance( "4", Int32.Parse( Utility.CurrentWeek() ), "RR", "PK" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestRantsTightEnds()
      {
         var fileOut = WeeklyEspnPerformance( "3", Int32.Parse( Utility.CurrentWeek() ), "RR", "TE" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestRantsWideReceivers()
      {
         var fileOut = WeeklyEspnPerformance( "3", Int32.Parse( Utility.CurrentWeek() ), "RR", "WR" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestRantsQbs()
      {
         var fileOut = WeeklyEspnPerformance( "1", Int32.Parse( Utility.CurrentWeek() ), "RR", "QB" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestYahooRunningBacks()
      {
         var fileOut = WeeklyEspnPerformance( "2", Int32.Parse( Utility.CurrentWeek() ), "YH", "RB" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestYahooKickers()
      {
         var fileOut = WeeklyEspnPerformance( "4", Int32.Parse( Utility.CurrentWeek() ), "YH", "PK" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestYahooTightEnds()
      {
         var fileOut = WeeklyEspnPerformance( "3", Int32.Parse( Utility.CurrentWeek() ), "YH", "TE" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestYahooWideReceivers()
      {
         var fileOut = WeeklyEspnPerformance( "3", Int32.Parse( Utility.CurrentWeek() ), "YH", "WR" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestYahooQbs()
      {
         var fileOut = WeeklyEspnPerformance( "1", Int32.Parse( Utility.CurrentWeek() ), "YH", "QB" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestBeastModeRunningBacks()
      {
         var fileOut = WeeklyEspnPerformance( "2", Int32.Parse( Utility.CurrentWeek() ), "TN", "RB" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestBeastModeKickers()
      {
         var fileOut = WeeklyEspnPerformance( "4", Int32.Parse( Utility.CurrentWeek() ), "TN", "PK" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestBeastModeTightEnds()
      {
         var fileOut = WeeklyEspnPerformance( "9", Int32.Parse( Utility.CurrentWeek() ), "TN", "TE" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestBeastModeWideReceivers()
      {
         var fileOut = WeeklyEspnPerformance( "3", Int32.Parse( Utility.CurrentWeek() ), "TN", "WR" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private string TestBeastModeQbs()
      {
         var fileOut = WeeklyEspnPerformance( "1", Int32.Parse( Utility.CurrentWeek() ), "TN", "QB" );
         Assert.IsTrue( File.Exists( fileOut ) );
         return fileOut;
      }

      private static string WeeklyEspnPerformance(
         string catCode, int week, string leagueId, [System.Runtime.InteropServices.Optional] string sPos )
      {
         var pl = new PlayerLister { WeeksToGoBack = 1 };
         var master = new YahooMaster( "Yahoo", "YahooOutput.xml" );
         var currentWeek =
            new NFLWeek( Int32.Parse( Utility.CurrentSeason() ), weekIn: week, loadGames: false );
         var gs = new EspnScorer( currentWeek ) { Master = master };

         pl.SetScorer( gs );
         pl.SetFormat( "weekly" );
         pl.AllWeeks = false; //  just the regular saeason
         pl.Season = currentWeek.Season;
         pl.RenderToCsv = false;
         pl.Week = week;
         pl.Collect( catCode, sPos: sPos, fantasyLeague: leagueId );

         var targetFile = string.Format( "{4}{3}//Performance//{2}-Yahoo {1} Performance upto Week {0}.htm",
            currentWeek.WeekNo, sPos, leagueId, pl.Season, Utility.OutputDirectory() );
         pl.Render( targetFile );
         return pl.FileOut;
      }
   }
}