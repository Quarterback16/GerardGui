using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class GeneratePlayerProjectionsTests
   {
      [TestMethod]
      public void TestCurrentGeneratePlayerProjectionsJob()   //  4 min
      {
         //  upcoming week only
         var sut = new GeneratePlayerProjectionsJob(new TimeKeeper(clock:null));
         var resultOut = sut.DoJob();
         Assert.IsTrue(resultOut.Length > 0);
      }

      [TestMethod]
      public void TestGeneratePlayerProjectionsJob()   // 85 mins 2016-08-21  (make sure debug mode is on)
      {
         var sut = new GeneratePlayerProjectionsJob( new FakeTimeKeeper( isPreSeason:true, isPeakTime:false) );
         var resultOut = sut.DoJob();
         Assert.IsTrue( resultOut.Length > 0 );
      }

      [TestMethod]
      public void TestTimetoDoGeneratePlayerProjectionsJob()
      {
         var sut = new GeneratePlayerProjectionsJob( new TimeKeeper( clock: null ) );
         string whyNot;
         var result = sut.IsTimeTodo( out whyNot );
         Assert.IsTrue(result);
         Console.WriteLine(whyNot);
      }

      [TestMethod]
      public void TestSingleGameProjection()  //  17 sec  2015-08-11
      {
         var game = new NFLGame( "2016:18-C" );  //  MD@PS
         var ppg = new PlayerProjectionGenerator(playerCache:null);
         ppg.Execute( game );
         Assert.IsTrue(  ppg != null );
      }

      [TestMethod]
      public void TestGettingTheKicker()
      {
         var team = new NflTeam( "PS" );
         team.SetKicker();
         Assert.AreEqual( expected: "Chris Boswell", actual: team.Kicker.PlayerName );
      }
   }
}