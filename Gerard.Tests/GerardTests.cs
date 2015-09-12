using Butler;
using Butler.Helpers;
using Butler.Models;
using Helpers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class GerardTests
   {
      [TestMethod]
      public void TestDoFantasyProjectionJob()
      {
         var sut = new FantasyProjectionJob( new TimeKeeper() );
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
      }

      [TestMethod]
      public void TestTimetoDoBalanceReport()
      {
         var sut = new BalanceReportJob(new TimeKeeper());
         string whyNot;
         Assert.IsFalse(sut.IsTimeTodo(out whyNot));
      }

      [TestMethod]
      public void TestDiskDetector()
      {
         var sut = new DiskDetector();
         var vm = sut.DisksView;
         foreach (var disk in vm.Disks)
         {
            Console.WriteLine("{0}:{1}", "Name", disk.Name);
         }
         Assert.IsTrue(vm.Disks.Count > 0);
      }

      [TestMethod]
      public void TestDiskAvailable()
      {
         var sut = new DiskDetector();
         Assert.IsFalse(sut.IsDiskAvailable("<diskId>"));
      }

      [TestMethod]
      public void TestFaMarketGetTeams()
      {
         var sut = new FaMarket();
         Assert.IsTrue(sut.GetTeams().Rows.Count > 0);
      }

      [TestMethod]
      public void TestDiskIdentifiers()
      {
         var sut = new DiskDetector();
         var ids = sut.DiskIdentifiers();
         Console.WriteLine("{0} : {1}", "IDs", ids);
         Assert.IsTrue(ids.Length > 0);
      }

      [TestMethod]
      public void TestTeamCardSf()
      {
         var t = Masters.Tm.GetTeam("2014", "SF");
         var fileOut = t.RenderTeamCard();
         Assert.IsTrue(File.Exists(fileOut));
      }

      [TestMethod]
      public void TestMediaJobLocal()
      {
         var sut = new MediaJob(@".\TestFolder\");
         Assert.IsTrue(sut.Candidates.Count > 0);
      }

      [TestMethod]
      public void TestMediaInfo()
      {
         var sut = new MediaInfo(@".\TestFolder\Judge.Judy.S14E157.2010.02.25.mp4");
         sut.Analyse();
         Assert.IsFalse(sut.IsMovie);
         Assert.IsTrue(sut.IsTV);
         Assert.IsTrue(sut.Season == 14);
         Assert.IsTrue(sut.Episode == 157);
         Assert.IsTrue(sut.Title == "Judge Judy");
      }

      [TestMethod]
      public void TestMediaInfoContainsNumber()
      {
         var sut = new MediaInfo(@".\TestFolder\Judge.Judy.S14E157.2010.02.25.mp4");
         sut.Analyse();
         Assert.IsTrue(sut.ContainsNumber("SGD22"));
         Assert.IsFalse(sut.ContainsNumber("Steve"));
      }

      [TestMethod]
      public void TestMediaInfo2()
      {
         var sut = new MediaInfo(@".\TestFolder\Game.of.Thrones.S04E07.HDTV.x264-KILLERS.[VTV].mp4");
         sut.Analyse();
         Assert.IsTrue(sut.IsTV);
         Assert.IsTrue(sut.Season == 4);
         Assert.IsTrue(sut.Episode == 7);
         Assert.IsTrue(sut.Title == "Game of Thrones");
      }

      [TestMethod]
      public void TestCollectorHasIt()
      {
         var mi = new MediaInfo(@".\TestFolder\Judge.Judy.S14E157.2010.02.25.mp4");
         mi.Analyse();
         var sut = new Collector();
         var hasIt = sut.HaveIt(mi);
         Assert.IsTrue(hasIt);
      }

      [TestMethod]
      public void TestAddingToCollection()
      {
         var mi = new MediaInfo(@".\TestFolder\Judge.Judy.S14E157.2010.02.25.mp5");
         mi.Analyse();
         var sut = new Collector();
         var result = sut.AddToTvCollection(mi);
         Assert.IsTrue(result.Equals(sut.LatestAddition));
      }




      public void TestUnitReportNiners()
      {
         //  Fake historian garantees job will run always
         var sut = new NflTeam("SF");
         var r = sut.PoReport();
         Assert.IsTrue(r.Length > 0);
      }

      [TestMethod]
      public void TestHistorian()
      {
         var sut = new Historian();
         var theDate = sut.LastRun(new OldRosterGridReports());
         Assert.AreEqual(new DateTime(2013, 11, 3).Date, theDate.Date);
      }

      [TestMethod]
      public void TestDataIntegrityCheckerWeek2013_1()
      {
         var sut = new DataIntegrityChecker();
         sut.Season = "2013";
         sut.Week = 1;
         sut.CheckScores();
         Assert.IsTrue(sut.ScoresChecked > 0);
      }

      [TestMethod]
      public void TestDataIntegrityChecker2013()
      {
         var sut = new DataIntegrityChecker();
         sut.Season = "2013";
         sut.CheckScores();
         Assert.IsTrue(sut.ScoresChecked > 0 && sut.Errors == 0);
      }

      [TestMethod]
      public void TestDataIntegrityStatsChecker2013()
      {
         var sut = new DataIntegrityChecker();
         sut.Season = "2013";
         sut.CheckStats();
         Assert.IsTrue(sut.StatsChecked > 0 && sut.Errors == 0);
      }

      [TestMethod]
      public void TestStatReport1()
      {
         var sut = new DataIntegrityChecker();
         sut.Season = "2013";
         sut.CheckStats();
         Assert.IsTrue(sut.StatsChecked > 0 && sut.Errors == 0);
      }

      [TestMethod]
      public void TestStatGrids()
      {
         var sut = new StatGrids("2013");
         sut.RenderAsHtml();
         sut.DumpXml();
         var fileOut = sut.OutputFilename();
         Assert.IsTrue( File.Exists( fileOut ));
      }

      [TestMethod]
      public void TestFpProjections()
      {
         var sut = new FpProjections();
         sut.RenderAsHtml();
         var fileOut = sut.OutputFilename();
         Assert.IsTrue(File.Exists(fileOut));
      }



      [TestMethod]
      public void TestProjectionKaepernick()
      {
         var sut = new NFLPlayer("KAEPCO01");
         var fileOut = sut.PlayerProjection("2014");
         Assert.IsTrue(File.Exists(fileOut));
      }

      [TestMethod]
      public void TestPredictedOutput()
      {
         var sut = new ScoreTally("2015", "All Teams", true);
         sut.Render();
         var fileOut = sut.FileName();
         Assert.IsTrue(File.Exists(fileOut), string.Format("Cannot find {0}", fileOut));
      }

      [TestMethod]
      public void TestActualOutput()
      {
         var sut = new ScoreTally("2014", "Actuals", usingPredictions: false);
         sut.ForceRefresh = false;
         sut.Render();
         var fileOut = sut.FileName();
         Assert.IsTrue(File.Exists(fileOut), string.Format("Cannot find {0}", fileOut));
      }

      [TestMethod]
      public void TestUnitPredictorPredictGame()
      {
         var predictor = new UnitPredictor
         {
            TakeActuals = true,
            AuditTrail = true,
            WriteProjection = false,
            StorePrediction = false,
            RatingsService = new UnitRatingsService()
         };
         var game = new NFLGame("2014:01-M");  //  SF @ DC
         var result = predictor.PredictGame(game, new FakePredictionStorer(), Utility.StartOfSeason());
         Assert.IsTrue(result.HomeWin());
         Assert.IsTrue(result.HomeScore.Equals(20), string.Format("Home score should be 20 not {0}", result.HomeScore));
         Assert.IsTrue(result.AwayScore.Equals(17), string.Format("Away score should be 17 not {0}", result.AwayScore));
      }
   }
}