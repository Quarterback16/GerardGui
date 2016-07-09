using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class MediaLogDetectorTests
   {
      #region  Sut Initialisation

      private MediaLogDetector sut;

      [TestInitialize]
      public void TestInitialize()
      {
         sut = SystemUnderTest();
      }

      private static MediaLogDetector SystemUnderTest()
      {
         return new MediaLogDetector();
      }

      #endregion

      [TestMethod]
      public void TestLogfileReadyToGo()
      {
         var result = sut.MediaFileMatches(
            logType: "Latest-Regina",
            fileName: ".\\medialists\\Latest-Regina.htm",
            logDate: new DateTime( 2016, 7, 5 ) );

         Assert.IsTrue( result );
      }

      [TestMethod]
      public void TestRightMediaLogFile()
      {
         var result = sut.MediaFileMatches(
            logType: "Latest-Regina",
            fileName: ".\\medialists\\TV-Regina.htm",
            logDate: new DateTime( 2016, 7, 5 ) );

         Assert.IsFalse( result );
      }

      [TestMethod]
      public void TestFileDate()
      {
         var result = sut.MediaFileDate(
            fileName: ".\\medialists\\Latest-Regina.htm"
            );

         Assert.AreEqual( expected: new DateTime( 2016, 7, 6 ).Date, actual: result.Date );
      }

   }
}
