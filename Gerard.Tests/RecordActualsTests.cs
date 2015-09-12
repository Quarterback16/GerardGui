using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class RecordActualsTests
   {
      [TestMethod]
      public void TestDoRecordActualMetricsJob()
      {
         var sut = new RecordActualMetricsJob(new FakeTimeKeeper( "2014", "01" ));
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
      }

      [TestMethod]
      public void TestPlayerTallying()
      {
         var sut = new NFLPlayer("KAEPCO01");
         sut.TallyScores("2014", 1);
         Console.WriteLine(sut.CurrentGameMetrics);
         Assert.IsTrue( sut.CurrentGameMetrics.TDp == 2);
         Assert.IsTrue(sut.CurrentGameMetrics.TDr == 0);
         Assert.IsTrue(sut.CurrentGameMetrics.TDc == 0);
      }

      [TestMethod]
      public void TestPlayerTallyingFieldGoals()
      {
         var sut = new NFLPlayer("BAILDA01");
         sut.TallyScores("2014", 1);
         Console.WriteLine(sut.CurrentGameMetrics);
         Assert.IsTrue(sut.CurrentGameMetrics.TDp == 0);
         Assert.IsTrue(sut.CurrentGameMetrics.TDr == 0);
         Assert.IsTrue(sut.CurrentGameMetrics.TDc == 0);
         Assert.IsTrue(sut.CurrentGameMetrics.FG == 1);
         Assert.IsTrue(sut.CurrentGameMetrics.Pat == 2);
      }

      [TestMethod]
      public void TestPlayerTallyingPassingYards()
      {
         var sut = new NFLPlayer("KAEPCO01");
         sut.TallyStats("2014", 1);
         Console.WriteLine(sut.CurrentGameMetrics);
         Assert.IsTrue(sut.CurrentGameMetrics.YDp == 201);
      }

      [TestMethod]
      public void TestPlayerTallyingRushingYards()
      {
         var sut = new NFLPlayer("KAEPCO01");
         sut.TallyStats("2014", 1);
         Console.WriteLine(sut.CurrentGameMetrics);
         Assert.IsTrue(sut.CurrentGameMetrics.YDr == 11);
      }

      [TestMethod]
      public void TestPlayerRecordingMetrics()
      {
         var season = "2014";
         var week = "01";
         var gamecode = "M";
         var game = new NFLGame( string.Format("{0}:{1}-{2}", season, week, gamecode) );
         var sut = new RecordOfActualMetricsReport(season, week);
         var player = new NFLPlayer("KAEPCO01");
         player.TallyScores(season, Int32.Parse(week) );
         player.TallyStats(season, Int32.Parse(week) );
         Console.WriteLine(player.CurrentGameMetrics);
         var result = sut.RecordMetrics(player, game);
         Assert.IsTrue( result.Substring( 0, 4 ) != "Fail");

      }

   }
}
