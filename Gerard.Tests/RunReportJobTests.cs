using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
   [TestClass]
   public class RunReportJobTests
   {
      [TestMethod]
      public void TestDoRunReportJob()
      {
         var sut = new RunReportJob( new FakeTimeKeeper() );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }

		[TestMethod]
		public void TestTimeToDoGameProjectionJob()  //     
		{
			var sut = new RunReportJob( new FakeTimeKeeper( season: "2015", week: "00" ) );
			string whyNot;
			var outcome = sut.IsTimeTodo( out whyNot );
			Console.WriteLine( whyNot );
			Assert.IsTrue( outcome );
		}
   }
}