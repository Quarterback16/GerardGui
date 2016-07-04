using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class LogMasterTests
   {
      [TestMethod]
      public void TestLoad()
      {
         var lm = new LogMaster(".\\xml\\mail-list.xml");

         Assert.IsTrue( lm.TheHT.Count  > 0);
      }

   }
}
