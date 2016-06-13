using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Helpers.Interfaces;
using Helpers;

namespace Gerard.Tests
{
   [TestClass]
   public class MediaMailerJobTests
   {
      [TestMethod]
      public void TestMediaMailerJob()
      {
         IMailMan mailMan = new MailMan2();
         IDetectLogFiles logFileDetector = new MediaLogDetector();
         var sut = new MediaMailerJob(mailMan, logFileDetector);
         sut.DoJob();
         Assert.IsTrue(sut.LogsMailed > 0);
      }
   }
}
