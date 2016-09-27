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
         var sut = new GeneratePlayerProjectionsJob(new TimeKeeper(null));
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
         var sut = new GeneratePlayerProjectionsJob(new FakeTimeKeeper());
         string whyNot;
         Assert.IsTrue(sut.IsTimeTodo(out whyNot));
         Console.WriteLine(whyNot);
      }

      [TestMethod]
      public void TestSingleGameProjection()  //  17 sec  2015-08-11
      {
         var game = new NFLGame( "2015:01-A" );  
         var ppg = new PlayerProjectionGenerator(null);
         ppg.Execute( game );
         Assert.IsTrue(  ppg != null );
      }
   }
}