using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler.Models;
using RosterLib;

namespace Gerard.Tests
{
    [TestClass]
   public class HotListTests
   {
      [TestMethod]
      public void TestDoHotlistsJob()  //  2015-11-08  1 min
      {
         var sut = new HotListsJob(new TimeKeeper(null));
         var outcome = sut.Execute();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
      }

   }
}
