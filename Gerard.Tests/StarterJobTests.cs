using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class StarterJobTests
   {
      [TestMethod]
      public void TestTimetoDoStartersReport()
      {
         var sut = new StartersJob( new TimeKeeper() );
         string whyNot;
         Assert.IsFalse( sut.IsTimeTodo( out whyNot ) );
      }

      [TestMethod]
      public void TestStartersJob()
      {
         var sut = new StartersJob( new TimeKeeper() );
         string whyNot;
         if (sut.IsTimeTodo( out whyNot ))
            sut.DoJob();
         var run = sut.Report.LastRun;
         Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
      }
   }
}