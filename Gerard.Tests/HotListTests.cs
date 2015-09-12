using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler.Models;

namespace Gerard.Tests
{
    [TestClass]
   public class HotListTests
   {
      [TestMethod]
      public void TestDoHotlistsJob()
      {
         var sut = new HotListsJob();
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
      }

   }
}
