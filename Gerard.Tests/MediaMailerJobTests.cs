using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
         var configReader = new ConfigReader();
         IMailMan mailMan = new MailMan2( configReader );
         IDetectLogFiles logFileDetector = new MediaLogDetector();
         var sut = new MediaMailerJob(mailMan, logFileDetector, configReader);
         sut.DoJob();
         Assert.IsTrue(sut.LogsMailed > 0);
      }
   }
}
