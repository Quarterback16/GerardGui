using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class OldRosterGridTests
   {
      [TestMethod]
      public void TestOldRosterGridJob()
      {
         var sut = new OldRosterGridJob( new FakeTimeKeeper() );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }
   }
}