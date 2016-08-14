using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class GameProjectionReportsJobTests
   {
      [TestMethod]
      public void TestCurrentGeneratePlayerProjectionsJob()   
      {
         var sut = new GameProjectionReportsJob( new TimeKeeper() );
         var resultOut = sut.DoJob();
         Assert.IsTrue( resultOut.Length > 0 );
      }
   }
}
