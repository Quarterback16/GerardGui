using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using RosterLib.RosterGridReports;

namespace Gerard.Tests
{
   [TestClass]
   public class PickupChartTests
   {
      [TestMethod]
      public void TestDoPickupChartJob()  //  1 min on 2015-09-01, 10 min with Projection Genrations turn on
      {
         var sut = new PickupChartJob( new FakeTimeKeeper(season:"2017",week:"01") );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

      [TestMethod]
      public void TestTimetoDoPickupChartReport()
      {
         var sut = new PickupChartJob( new FakeTimeKeeper( isPreSeason:true, isPeakTime: true ) );
         var result = sut.IsTimeTodo( out string whyNot );
         if ( !string.IsNullOrEmpty( whyNot ) )
            Console.WriteLine(whyNot);
         Assert.IsFalse( result );
      }

      [TestMethod]
      public void TestTimetoDoPickupChartReportNow()
      {
         var sut = new PickupChartJob( 
            new TimeKeeper( 
               clock:new FakeClock(new DateTime(2017,2,26,3,0,0) )) );
         var result = sut.IsTimeTodo( out string whyNot );
         if ( !string.IsNullOrEmpty( whyNot ) )
            Console.WriteLine( whyNot );
         Assert.IsTrue( result );
      }


      [TestMethod]
      public void TestDoPickupChartJobNow()
      {
         var sut = new PickupChartJob( new TimeKeeper(null) );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

      [TestMethod]
      public void TestDoPreviousPickupChartJobNow()
      {
         var sut = new PickupChartJob( new TimeKeeper( null ), previous: true );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

      [TestMethod]
		public void TestActualOutput()
		{
			var sut = new PickupChart( new FakeTimeKeeper( season: "2015"), week: 12);
			var p = new NFLPlayer("STAFMA01");
			var g = new NFLGame("2015:12-A");
			var result = sut.ActualOutput(g, p);
			Assert.AreEqual(expected:" 34 ",actual:result);
		}

		[TestMethod]
		public void TestProjectedOutput()
		{
			var sut = new YahooCalculator();
			var p = new NFLPlayer("STAFMA01");
			var g = new NFLGame("2015:12-A");
			var result = sut.Calculate(p, g);
			Assert.AreEqual(expected: 20, actual: p.Points);
		}


		[TestMethod]
		public void TestGameHasBeenPlayed()
		{
			var g = new NFLGame("2015:12-D");
			Assert.IsTrue(g.Played());
		}

      [TestMethod]
      public void TestCurrentWeek()
      {
         var sut = new PickupChartJob( new FakeTimeKeeper() );
         Console.WriteLine( "Week is {0}", sut.Week );
         Assert.IsTrue( sut.Week == 0 );
      }

      [TestMethod]
      public void TestJamalCharlesWeek1_2016()
      {
         var c = new YahooCalculator();
         var sut = new PickupChart( new FakeTimeKeeper( season: "2016" ), week: 1 );
         var p = new NFLPlayer( "CHARJA01" );
         var g = new NFLGame( "2016:01-G" );
         var result = sut.PlayerPiece( p, g, c );
         Console.WriteLine( "Piece is {0}", result );
      }

      [TestMethod]
		public void TestLoadPassingUnit()
		{
			var sut = new NflTeam( "KC" );
			var passingUnit = sut.LoadPassUnit();
			Console.WriteLine( "Passing Unit is {0}", passingUnit );
		}

      [TestMethod]
      public void TestLoadRushingUnit()
      {
         var sut = new NflTeam( "KC" );
         var unit = sut.LoadRushUnit();
         Console.WriteLine( "Rushing Unit is {0}", unit );
      }

      [TestMethod]
      public void TestPredictedResult()
      {
         var sut = new NFLGame( "2014:16-G" );
         var prediction = sut.PredictedResult();
         Assert.AreEqual( "CP 20-CL 17", prediction );
      }

		[TestMethod]
		public void TestGamePlayed()
		{
			var sut = new NFLGame("2015:10-A");
			Assert.IsFalse(sut.Played());
		}

      [TestMethod]
      public void TestBookiePredictedResult()
      {
         var sut = new NFLGame( "2016:04-O" );
         sut.CalculateSpreadResult();
         var predictedResult = sut.BookieTip.PredictedScore();
         Assert.AreEqual( "MV*24vNG*20", predictedResult );
      }

      [TestMethod]
      public void TestBookiePredictedResultPickEmGame()
      {
         var sut = new NFLGame("2015:11-B");
         sut.CalculateSpreadResult();
         var predictedResult = sut.BookieTip.PredictedScore();
         Assert.AreEqual("DL 25vOR 24", predictedResult);
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

      [TestMethod]
      public void TestW2Bit()
      {
         var sut = new PickupChart( new FakeTimeKeeper( season: "2016" ), week:15 );
         var g = new NFLGame( "2016:15-M" );
         var team = new Winner
         {
            Team = g.Team( "NE" ),
            Margin = Math.Abs( g.Spread ),
            Home = g.IsHome( "NE" ),
            Game = g
         };
         team.Team.LoadPassUnit();
         team.Team.PassUnit.SetReceiverRoles();
         var bit = sut.GetW2Bit( team, new YahooCalculator() );
         Assert.AreEqual( "CHogan", team.Team.PassUnit.W2.PlayerNameShort );
      }

      [TestMethod]
      public void TestAjayiWeek16_2016()
      {
         var c = new YahooCalculator();
         var sut = new PickupChart( new FakeTimeKeeper( season: "2016" ), week: 16 );
         var p = new NFLPlayer( "AJAYJA01" );
         var g = new NFLGame( "2016:16-B" );
         var result = sut.PlayerPiece( p, g, c );
         Console.WriteLine( "Piece is {0}", result );
      }

   }
}
