using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Helpers.Interfaces;
using Helpers;

namespace Gerard.Tests
{
   [TestClass]
   public class LogMailerJobTests
   {
      [TestMethod]
      public void TestLogMailerJob()
      {
         IMailMan mailMan = new MailMan2();
         IDetectLogFiles logFileDetector = new LogFileDetector();
         var sut = new LogMailerJob( mailMan, logFileDetector );
         sut.DoJob();
         Assert.IsTrue(sut.LogsMailed > 0);
      }
   }
}
