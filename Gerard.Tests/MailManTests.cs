using Microsoft.VisualStudio.TestTools.UnitTesting;
using Helpers;

namespace Gerard.Tests
{
   [TestClass]
   public class MailManTests
   {
      private MailMan2 sut;

      [TestInitialize]
      public void TestInitialize()
      {
         sut = SystemUnderTest();
      }

      private static MailMan2 SystemUnderTest()
      {
         return new MailMan2();
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
