using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helpers;

namespace Gerard.Tests
{
   [TestClass]
   public class MailManTests
   {
      private MailMan sut;

      [TestInitialize]
      public void TestInitialize()
      {
         sut = SystemUnderTest();
      }

      private static MailMan SystemUnderTest()
      {
         return new MailMan();
      }

      [TestMethod]
      public void TestSendMail()
      {
         sut.SendMail( message:"Test", subject:"Test" );
      }

      [TestMethod]
      public void TestSendMailWithAttachment()
      {
         sut.SendMail(message: "Test", subject: "Test", attachment: "l:\\RssScanner-2015-09-14.log");
      }

      [TestMethod]
      public void TestPokeServer()
      {
         MailMan.PokeServer("192.168.1.113", 25);
      }
   }
}
