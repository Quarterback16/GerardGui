using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class WeekMasterTests
   {
      [TestMethod]
      public void TestGetFromMaster()
      {
         var sut = new WeekMaster();
         var w = sut.GetWeek( "2014", 1 );
         Assert.IsTrue(w.WeekKey().Equals( "2014:01" ) );
         w = sut.GetWeek("2014", 1);
         Assert.IsTrue(sut.CacheHits == 1);
         Assert.IsTrue(sut.CacheMisses == 1);
      }
   }
}
