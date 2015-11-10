﻿using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class PickupChartTests
   {
      [TestMethod]
      public void TestDoPickupChartJob()  //  1 min on 2015-09-01
      {
         var sut = new PickupChartJob( new FakeTimeKeeper( season: "2015", week:"09" ) );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

      [TestMethod]
      public void TestCurrentWeek()
      {
         var sut = new PickupChartJob( new FakeTimeKeeper() );
         Console.WriteLine( "Week is {0}", sut.Week );
         Assert.IsTrue( sut.Week == 0 );
      }


		[TestMethod]
		public void TestLoadPassingUnit()
		{
			var sut = new NflTeam( "NJ" );
			var passingUnit = sut.LoadPassUnit();
			Console.WriteLine( "Passing Unit is {0}", passingUnit );
		}


      [TestMethod]
      public void TestPredictedResult()
      {
         var sut = new NFLGame( "2014:16-G" );
         var prediction = sut.PredictedResult();
         Assert.AreEqual( "CP 20-CL 17", prediction );
      }

      [TestMethod]
      public void TestBookiePredictedResult()
      {
         var sut = new NFLGame( "2014:16-G" );
         sut.CalculateSpreadResult();
         var predictedResult = sut.BookieTip.PredictedScore();
         Assert.AreEqual( "CP 23-CL 17", predictedResult );
      }

      [TestMethod]
      public void TestMarginOfVictory()
      {
         var sut = new NFLGame( "2014:16-H" );
         sut.CalculateSpreadResult();
         var marginOfVictory = sut.MarginOfVictory();
         Assert.AreEqual( 7, marginOfVictory );
      }

      [TestMethod]
      public void TestFavouriteTeam()
      {
         var sut = new NFLGame( "2014:16-H" );
         sut.CalculateSpreadResult();
         Assert.AreEqual( "NO", sut.FavouriteTeam().TeamCode );
      }

      [TestMethod]
      public void TestPlayerNameTo()
      {
         var sut = new NFLPlayer( "ROETBE01" );
         var sn = sut.PlayerNameTo( 10 );
         Assert.AreEqual( "BRoethlisb", sn );
      }

		[TestMethod]
		public void TestKicker()
		{
			var sut = new NflTeam("SF");
			var sn = sut.LoadKickUnit();
			Assert.IsNotNull(sut.KickUnit);
			Assert.IsNotNull(sut.KickUnit.PlaceKicker);
		}

		[TestMethod]
		public void TestKickerCalcs()
		{
			var sut = new YahooCalculator();
			var p = new NFLPlayer("GOSTST01");
			var g = new NFLGame("2015:09-C");
			sut.Calculate(p, g);
			Assert.IsTrue(p.Points > 1);
		}
   }
}
