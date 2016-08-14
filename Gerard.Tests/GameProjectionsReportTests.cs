using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerard.Tests
{
   [TestClass]
   public class GameProjectionsReportTests
   {
      [TestMethod]
      public void TestGameProjectionsReport() 
      {
         var cut = new GameProjectionsReport( new FakeTimeKeeper("2016","00") );
         Assert.IsNotNull( cut );
         cut.RenderAsHtml();
      }
   }
}
