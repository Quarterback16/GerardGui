using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerard.Tests
{
   [TestClass]
   public class LogDirectoryFinderTests
   {
      #region  Sut Initialisation

      private LogDirectoryFinder sut;

      [TestInitialize]
      public void TestInitialize()
      {
         sut = SystemUnderTest();
      }

      private static LogDirectoryFinder SystemUnderTest()
      {
         return new LogDirectoryFinder();
      }

      #endregion

      [TestMethod]
      public void TestLogfilesUnderTest()
      {
         var result = sut.GetLogDirectories();
         Assert.AreEqual(expected: 1, actual: result.Count);
      }
   }
}
