using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class OldRosterGridTests
   {
      [TestMethod]
      public void TestOldRosterGridJob()
      {
         var sut = new OldRosterGridJob( new TimeKeeper() );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }
   }
}