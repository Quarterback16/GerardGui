using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class ButlerProcessingTests
   {
      [TestMethod]
      public void TestDoAssignRolesJob()
      {
         var sut = new AssignRolesJob( new FakeTimeKeeper() );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }
   }
}
