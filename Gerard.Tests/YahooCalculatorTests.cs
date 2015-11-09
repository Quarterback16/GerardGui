using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using TFLLib;
using System.IO;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class YahooCalculatorTests
   {
      [TestMethod]
      public void TestYahooProjectedPointsForPeytonManning2014Week01()
      {
         var p = new NFLPlayer( "NEWTCA01" );
         var g = new NFLGame( "2015:08-N" );
         g.LoadPrediction();
         Console.WriteLine( "{0} {1}", p.PlayerNameShort, p.StatsFor( g, "CP" ) );
         var c = new YahooCalculator();
         var msg = c.Calculate( p, g );
         var expected = 6;  //  0 TDp and 150 YDp
         Assert.AreEqual( expected, msg.Player.Points );
      }

      [TestMethod]
      public void TestYahooProjectedPointsFrTomBrady2013Week01()
      {
         var p = new NFLPlayer( "BRADTO01" );
         var g = new NFLGame( "2013:01-B" );
         g.LoadPrediction();
         var c = new YahooCalculator();
         var msg = c.Calculate( p, g );
         var expected = 18;  //  2 TDp and 250 YDp
         Assert.AreEqual( expected, msg.Player.Points );
      }

      [TestMethod]
      public void TestYahooProjectedPointsForSammyWatkinsWeek16()
      {
         var p = new NFLPlayer( "WATKSA01" );
         var g = new NFLGame( "2014:16-M" );
         g.LoadPrediction();
         var c = new YahooCalculator();
         var msg = c.Calculate( p, g );
         var expected = 13;  //  1 TDp and 75 YDc
         Assert.AreEqual( expected, msg.Player.Points );
      }

   }
}
