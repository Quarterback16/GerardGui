using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class GameProjectionTests
   {
      [TestMethod]
      public void TestRenderOfGameProjection()
      {
         var game = new NFLGame("2016:01-I");  //  CH @ HT
         var cut = new GameProjection( game );
         cut.Render();
         Assert.IsTrue( File.Exists( cut.FileName() ), 
            string.Format( "Cannot find {0}", cut.FileName() ) );
      }
   }
}
