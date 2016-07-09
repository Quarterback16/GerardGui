using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
         var configReader = new ConfigReader();
         IMailMan mailMan = new MailMan2(configReader);
         IDetectLogFiles logFileDetector = new LogFileDetector();
         var sut = new LogMailerJob( mailMan, logFileDetector, configReader );
         sut.DoJob();
         Assert.IsTrue(sut.LogsMailed > 0);
      }
   }
}
