using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using Butler.Models;

namespace Gerard.Tests
{
	[TestClass]
	public class DeletePlayerReportsTests
	{
		[TestMethod]
		public void TestPlayerReportsJob()
		{
			var sut = new DeletePlayerReportsJob( new TimeKeeper( null ) );
			sut.DoJob();
			var run = sut.Report.LastRun;
			Console.WriteLine( "Last Run : {0}", run );
			Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
		}

		[TestMethod]
		public void TestTimetoDeletePlayerReports()
		{
			var sut = new DeletePlayerReportsJob( new FakeTimeKeeper( isPreSeason: true, isPeakTime: false ) );
			var isTime = sut.IsTimeTodo( out string whyNot );
			Console.WriteLine( whyNot );
			Assert.IsFalse( isTime );
		}

		[TestMethod]
		public void TestSingleDelete()
		{
			bool actualresult;
			bool expectedResult;
			var sut = new NFLPlayer( "MOSSSA01" );
			if ( sut.IsPlayerReport() )
			{
				expectedResult = true;
			}
			else
				expectedResult = false;
			actualresult = sut.DeletePlayerReport();

			Assert.AreEqual( actualresult, expectedResult );
		}
	}
}
