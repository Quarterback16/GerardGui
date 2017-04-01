using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;


namespace Gerard.Tests
{
	[TestClass]
	public class TopDogReportJobTests
	{
		[TestMethod]
		public void TestDoTopDogReportJob()
		{
			var sut = new TopDogReportJob( new FakeTimeKeeper( "2015", "01" ) );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestTimetoDoTopDogReports()
		{
			var sut = new TopDogReportJob( new TimeKeeper( clock: null ) );
			string whyNot;
			Assert.IsFalse( sut.IsTimeTodo( out whyNot ) );
			Console.WriteLine( whyNot );
		}
	}
}
