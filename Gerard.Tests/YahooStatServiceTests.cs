using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib.Services;

namespace Gerard.Tests
{
   [TestClass]
   public class YahooStatServiceTests
   {
      [TestMethod]
      public void TestGetStatsForMattyIce()  
      {
         var sut = new YahooStatService();
         var result = sut.GetStat( "RYANMA01", "2016", "01" );
         Assert.AreEqual( expected: 22.0M, actual: result );
      }

      [TestMethod]
      public void TestAnyStatsForMattyIce()
      {
         var sut = new YahooStatService();
         var result = sut.IsStat( "RYANMA01", "2016", "17" );
         Assert.IsTrue( result );
      }
   }
}
