using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class ConfigReaderTests
   {
      private ConfigReader sut;

      [TestInitialize]
      public void TestInitialize()
      {
         sut = SystemUnderTest();
      }

      private static ConfigReader SystemUnderTest()
      {
         return new ConfigReader();
      }

      [TestMethod]
      public void TestGetSetting()
      {
         var testSetting = sut.GetSetting( "OutputDirectory" );
         Assert.IsFalse( string.IsNullOrEmpty( testSetting ) );
         Assert.AreEqual( expected: ".//Output//", actual: testSetting );
      }
   }
}
