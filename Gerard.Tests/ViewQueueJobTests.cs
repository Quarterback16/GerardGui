using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
    [TestClass]
    public class ViewQueueJobTests
    {
        [TestMethod]
        public void ViewQueueJob_Processes()
        {
            var cut = new ViewQueueJob();
            var result = cut.DoJob();
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}
