using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class OutputProjectionsTests
   {
      [TestMethod]
      public void TesOutputProjection()
      {
         var sut = new OutputProjectionsJob( new FakeHistorian() );
         var resultOut = sut.DoJob();
         Assert.IsTrue( resultOut.Length > 0 );
      }
   }
}
