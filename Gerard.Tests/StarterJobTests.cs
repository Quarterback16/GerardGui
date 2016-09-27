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
      public void TestStartersJob()  //  2015-11-17  6 mins (only QBs)
      {
         var sut = new StartersJob( new TimeKeeper(null) );
         sut.DoJob();
         var run = sut.Report.LastRun;
         Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
      }

		[TestMethod]
		public void TestTimetoDoStartersReport()
		{
			var sut = new StartersJob(new FakeTimeKeeper( season:"2015", week:"10" ) );
			string whyNot;
			Assert.IsTrue(sut.IsTimeTodo(out whyNot));
		}
   }
}