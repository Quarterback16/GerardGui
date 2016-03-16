using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
   }
}
