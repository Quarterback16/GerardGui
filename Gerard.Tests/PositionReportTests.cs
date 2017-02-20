using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class PositionReportTests
   {
      [TestMethod]
      public void TestTeReport()
      {
         var options = new PositionReportOptions();
         options.Topic = "Tight End";
         options.PositionAbbr = "TE";
         options.PosDelegate = IsTe;
         options.PositionCategory = Constants.K_RECEIVER_CAT;

         var sut = new PositionReport( 
            new FakeTimeKeeper( season: "2016", week: "17" ),
            options );
         sut.RenderAsHtml();
         Assert.IsTrue( File.Exists( sut.FileOut ) );
         Console.WriteLine( "{0} created.", sut.FileOut );
      }

      public bool IsTe( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_RECEIVER_CAT ) && p.Contains( "TE", p.PlayerPos );
      }

      [TestMethod]
      public void TestRbReport()
      {
         var options = new PositionReportOptions();
         options.Topic = "Running Back";
         options.PositionAbbr = "RB";
         options.PosDelegate = IsRb;
         options.PositionCategory = Constants.K_RUNNINGBACK_CAT;

         var sut = new PositionReport(
            new FakeTimeKeeper( season: "2016" ),
            options );
         sut.RenderAsHtml();
         Assert.IsTrue( File.Exists( sut.FileOut ) );
         Console.WriteLine( "{0} created.", sut.FileOut );
      }

      [TestMethod]
      public void TestWrReport()
      {
         var options = new PositionReportOptions();
         options.Topic = "Wide Receiver";
         options.PositionAbbr = "WR";
         options.PosDelegate = IsWr;
         options.PositionCategory = Constants.K_RECEIVER_CAT;

         var sut = new PositionReport(
            new FakeTimeKeeper( season: "2016" ),
            options );
         sut.RenderAsHtml();
         Assert.IsTrue( File.Exists( sut.FileOut ) );
         Console.WriteLine( "{0} created.", sut.FileOut );
      }

      [TestMethod]
      public void TestQbReport()
      {
         var options = new PositionReportOptions();
         options.Topic = "Quarterback";
         options.PositionAbbr = "QB";
         options.PosDelegate = IsQb;
         options.PositionCategory = Constants.K_QUARTERBACK_CAT;

         var sut = new PositionReport(
            new FakeTimeKeeper( season: "2016" ),
            options );
         sut.RenderAsHtml();
         Assert.IsTrue( File.Exists( sut.FileOut ) );
         Console.WriteLine( "{0} created.", sut.FileOut );
      }

      [TestMethod]
      public void TestPkReport()
      {
         var options = new PositionReportOptions();
         options.Topic = "Kicker";
         options.PositionAbbr = "PK";
         options.PosDelegate = IsPk;
         options.PositionCategory = Constants.K_KICKER_CAT;

         var sut = new PositionReport(
            new FakeTimeKeeper( season: "2016" ),
            options );
         sut.RenderAsHtml();
         Assert.IsTrue( File.Exists( sut.FileOut ) );
         Console.WriteLine( "{0} created.", sut.FileOut );
      }

      public bool IsWr( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_RECEIVER_CAT ) && p.Contains( "WR", p.PlayerPos );
      }

      public bool IsRb( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_RUNNINGBACK_CAT ) && p.Contains( "RB", p.PlayerPos );
      }

      public bool IsQb( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_QUARTERBACK_CAT ) && p.Contains( "QB", p.PlayerPos );
      }

      public bool IsPk( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_KICKER_CAT );
      }
   }
}
