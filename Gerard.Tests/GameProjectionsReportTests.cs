﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class GameProjectionsReportTests
   {
      [TestMethod]
      public void TestGameProjectionsReport() 
      {
         var cut = new GameProjectionsReport( new FakeTimeKeeper("2016","06") );
         Assert.IsNotNull( cut );
         cut.RenderAsHtml();
      }
   }
}
