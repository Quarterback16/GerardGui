using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler.Models;

namespace Gerard.Tests
{
   [TestClass]
   public class TflBackupTests
   {
      [TestMethod]
      public void TestTimetoDoTflBackupJob()
      {
         var sut = new TflDataBackupJob();
         string whyNot;
         Assert.IsFalse(sut.IsTimeTodo(out whyNot));
      }

      [TestMethod]
      public void TestDoTflBackupJob()
      {
         var sut = new TflDataBackupJob();
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
      }
   }
}
