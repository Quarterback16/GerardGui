using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
    [TestClass]
    public class TflBackupTests
    {
        [TestMethod]
        public void TestTimetoDoTflBackupJob()
        {
            var sut = new TflDataBackupJob("e:\\tfl");
            Assert.IsFalse(sut.IsTimeTodo(out _));
        }

        [TestMethod]
        public void TestDoTflBackupJob()
        {
            var sut = new TflDataBackupJob("e:\\tfl");
            var outcome = sut.DoJob();
            Assert.IsFalse(string.IsNullOrEmpty(outcome));
        }
    }
}
