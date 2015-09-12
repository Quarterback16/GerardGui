using System;
using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class RandomNumberGeneratorTests
   {
      [TestMethod]
      public void TestANumberBetweenOneAndTen()
      {
         var rn = RandomNumberGenerator.GetRandomIntBetween( 1, 10 );
         Console.WriteLine( "Random Number: {0}", rn );
         Assert.IsTrue( ( rn >= 1 ) && (rn <= 10) );
         Assert.IsTrue( ( rn == 7 ) );  //  because we are using the fixed seed
      }
   }
}
