using Butler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerard.Tests
{
   [TestClass]
   public class CollectorTests
   {
      [TestMethod]
      public void TestGetTVFolder()
      {
         var sut = new Collector();
         var tvFolder = sut.GetTvFolder();
         Assert.IsFalse( string.IsNullOrEmpty( tvFolder ) );
      }

      [TestMethod]
      public void TestGetViewQueueFolder()
      {
         var sut = new Collector();
         var folder = sut.GetViewQueueFolder();
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
         var folder = sut.GetSoccerFolder();
         Assert.IsFalse( string.IsNullOrEmpty( folder ) );
      }

   }
}
