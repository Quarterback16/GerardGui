using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib.Services;
using System.Linq;

namespace Gerard.Tests
{
   [TestClass]
   public class YahooStatServiceTests
   {
      [TestMethod]
      public void TestGetStatsForDerrickCarr()  
      {
         var sut = new YahooStatService();
         var result = sut.GetStat( "CARRDE01", "2016", "09" );
         Assert.AreEqual( expected: 7.66M, actual: result );
      }

      [TestMethod]
      public void TestAnyStatsForMattyIce()
      {
         var sut = new YahooStatService();
         var result = sut.IsStat( "RYANMA01", "2016", "17" );
         Assert.IsTrue( result );
      }

      [TestMethod]
      public void TestAallStatsForMattyIce()
      {
         var sut = new YahooStatService();
         var result = sut.LoadStats( "RYANMA01", "2016", "17" );
         Assert.IsTrue( result.Any() );
      }
   }
}
