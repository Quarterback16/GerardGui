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
      [TestMethod]
      public void TestSendMail()
      {
         MailMan.SendMail( message:"Test", subject:"Test" );
      }
   }
}
