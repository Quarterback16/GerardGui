using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class YahooProjectionScorerTests
   {
      [TestMethod]
      public void TestFantasyProjectionJob()  // 59 ms 2016-08-12 
      {
         var cut = new YahooProjectionScorer();

         var dummyWeek = new NFLWeek( "9999", 99, loadGames: false );
         var dummyPlayer = new NFLPlayer();
         dummyPlayer.ProjectedTDp = 1;

         cut.RatePlayer( dummyPlayer, dummyWeek );

         Assert.AreEqual( expected: 4, actual: dummyPlayer.Points );
      }
   }
}
