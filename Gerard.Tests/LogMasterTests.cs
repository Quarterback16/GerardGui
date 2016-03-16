using Helpers;
using Helpers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

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
