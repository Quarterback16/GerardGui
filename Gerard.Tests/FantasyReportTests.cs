using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class FantasyReportTests
   {
      [TestMethod]
      public void TestReport()
      {
         var sut = new FantasyReport( new FakeTimeKeeper(season:"2016", week: "14" ));
         sut.Render();
         Assert.IsTrue( File.Exists( sut.FileOut ) );
         Console.WriteLine( "{0} created.", sut.FileOut );
      }

      [TestMethod]
      public void TestGameResult()
      {
         var sut = new NFLGame( gameKey: "2016:14-D" );  
         var result = sut.ResultFor( teamInFocus: "NO", abbreviate: true, barIt: false );
         Assert.AreEqual( expected: "NO @ TB", actual: result );
      }

      [TestMethod]
      public void TestDefensiveFantasyPoints()
      {
         var sut = new NFLGame( gameKey: "2016:13-B" );  // KC @ AF
         var pts = sut.DefensiveFantasyPtsFor(teamCode:"KC");
         Assert.AreEqual( expected: 11.0M, actual: pts );
      }

      [TestMethod]
      public void TestDefensiveScoringCalculator()
      {
         var week = new NFLWeek( seasonIn: "2016", weekIn: "13" );
         var team = new NflTeam( "KC" );
         var game = new NFLGame( gameKey: "2016:13-B" );  // KC @ AF
         var sut = new DefensiveScoringCalculator( week, offset:0 );
         sut.Calculate( team: team, game: game );
         Assert.AreEqual( expected: 11.0M, actual: team.FantasyPoints );
      }
   }
}
