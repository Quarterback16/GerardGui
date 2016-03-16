using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerard.Tests
{
   [TestClass]
   public class XmlLoaderTests
   {
      #region  Sut Initialisation

      private XmlLoader sut;

      [TestInitialize]
      public void TestInitialize()
      {
         sut = SystemUnderTest();
      }

      private static XmlLoader SystemUnderTest()
      {
         return new XmlLoader();
      }

      #endregion

      [TestMethod]
      public void TestNullParameters()
      {
         var result = sut.LoadFromXml("", "");
         Assert.AreEqual(expected: 0, actual: result.Count);
      }

      [ExpectedException(typeof(System.IO.FileNotFoundException))]
      [TestMethod]
      public void TestInvalidFileName()
      {
         var result = sut.LoadFromXml( "invalid.log", "logfile");
         Assert.AreEqual(expected: 1, actual: result.Count);
      }

      [TestMethod]
      public void TestInvalidXmlFile()
      {
         var result = sut.LoadFromXml(".\\xml\\team.xml", "logfile");
         Assert.AreEqual(expected: 0, actual: result.Count);
      }

      [TestMethod]
      public void TestTeamXmlFile()
      {
         var result = sut.LoadFromXml(".\\xml\\team.xml", "team", "name");
         Assert.AreEqual(expected: 258, actual: result.Count);
      }

      [TestMethod]
      public void TestMailListXmlFile()
      {
         var result = sut.LoadFromXml(".\\xml\\mail-list.xml", "mail");
         Assert.AreEqual(expected: 1, actual: result.Count);
      }
   }
}
