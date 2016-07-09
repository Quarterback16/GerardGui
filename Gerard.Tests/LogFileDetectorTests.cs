using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class LogFileDetectorTests
   {
      #region  Sut Initialisation

      private LogFileDetector sut;

      [TestInitialize]
      public void TestInitialize()
      {
         sut = SystemUnderTest();
      }

      private static LogFileDetector SystemUnderTest()
      {
         return new LogFileDetector();
      }

      #endregion

      [TestMethod]
      public void TestLogfilesUnderTest()
      {
         var result = sut.DetectLogFileIn(".\\logs\\", "collector", new DateTime(2015,12,31));
         Assert.AreEqual(expected: 4, actual: result.Count);
      }

      [TestMethod]
      public void TestLogfileInProgress()
      {
         //  no match because the log file's date is the same as the current date 
         var result = sut.FileMatches( 
            dir: ".\\logs\\", 
            fileNameWithNoExtension: "ArticleScanner-2016-07-07", 
            logType: "ArticleScanner", 
            logDate: new DateTime( 2016, 7, 7 ));

         Assert.IsFalse( result );
      }

      [TestMethod]
      public void TestLogfileReadyToGo()
      {
         var result = sut.FileMatches(
            dir: ".\\logs\\",
            fileNameWithNoExtension: "ArticleScanner-2016-07-06",
            logType: "ArticleScanner",
            logDate: new DateTime( 2016, 7, 5 ) );

         Assert.IsTrue( result );
      }

      [TestMethod]
      public void TestFileDate()
      {
         var result = sut.FileDate(
            dir: ".\\logs\\",
            fileNameWithNoExtension: "ArticleScanner-2016-07-07") ;

         Assert.AreEqual( expected: new DateTime(2016,7,7), actual:result );
      }

      [TestMethod]
      public void TestFileDateIsGotFromFileName()
      {
         // as opposed to the last update date
         var result = sut.FileDate(
            dir: ".\\logs\\",
            fileNameWithNoExtension: "ArticleScanner-2016-07-06" );

         Assert.AreEqual( expected: new DateTime( 2016, 7, 6 ), actual: result );
      }

   }
}
