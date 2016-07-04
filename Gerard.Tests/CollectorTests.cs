using Butler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gerard.Tests
{
   [TestClass]
   public class CollectorTests
   {
      [TestMethod]
      public void TestGetTVFolder()
      {
         var sut = new Collector();
         var tvFolder = Collector.GetTvFolder();
         Assert.IsFalse( string.IsNullOrEmpty( tvFolder ) );
      }

      [TestMethod]
      public void TestGetViewQueueFolder()
      {
         var sut = new Collector();
         var folder = Collector.GetViewQueueFolder();
         Assert.IsFalse( string.IsNullOrEmpty( folder ) );
      }

      [TestMethod]
      public void TestGettingTheCollection()
      {
         var sut = new Collector();
         sut.LoadTvCollection();
         Assert.IsTrue( sut.TvCollection.Count() > 0 );
      }

      [TestMethod]
      public void TestGetSoccerFolder()
      {
         var sut = new Collector();
         var folder = Collector.GetSoccerFolder();
         Assert.IsFalse( string.IsNullOrEmpty( folder ) );
      }

   }
}
