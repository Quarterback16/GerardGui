using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class GameProjectionTests
   {
      [TestMethod]
      public void TestRenderOfGameProjection()
      {
         var game = new NFLGame("2020:09-H");  //  BR @ IC
         var cut = new GameProjection( game );
         cut.Render();

         Assert.IsTrue( File.Exists( cut.FileName() ), 
            $"Cannot find {cut.FileName()}" );
            Console.WriteLine($" rendered {cut.FileName()}");
      }
   }
}
