using RosterLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class UtilityTests
   {
      [TestMethod]
      public void TestCurrentWeek()
      {
         var cw = Utility.CurrentWeek();
         Assert.IsFalse( string.IsNullOrEmpty(cw) );
      }
   }
}
