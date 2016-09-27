using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class OldRosterGridTests
   {
      [TestMethod]
      public void TestOldRosterGridJob()  //  2015-12-09  2 mins
      {
         var sut = new OldRosterGridJob( new TimeKeeper(null) );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }
   }
}