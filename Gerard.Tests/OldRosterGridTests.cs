using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
	[TestClass]
   public class OldRosterGridTests
   {
      [TestMethod]
      public void TestOldRosterGridJob()  //  2015-12-09  2 mins
      {
         var sut = new OldRosterGridJob(
             new FakeTimeKeeper(
                 season:"2020") );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }
   }
}