using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class PlayerReportTests
	{
		[TestMethod]
		public void TestPlayerReportsJob()
		{
			var sut = new PlayerReportsJob(
                new TimeKeeper( null ),
                new FakeConfigReader() );
			sut.DoJob();
			var run = sut.Report.LastRun;
			Console.WriteLine( "Last Run : {0}", run );
			Assert.IsTrue(
                run.Date.Equals( DateTime.Now.Date ) );
		}

		[TestMethod]
		public void TestTimetoDoPlayerReports()
		{
			var sut = new PlayerReportsJob( 
                new FakeTimeKeeper( 
                    isPreSeason: true, 
                    isPeakTime: false ),
				new FakeConfigReader() );
			var isTime = sut.IsTimeTodo( out string whyNot );
			Console.WriteLine( whyNot );
			Assert.IsTrue( isTime );
		}

		[TestMethod]
		public void TestDeshaunWatson()
		{
			var sut = new NFLPlayer( "WATSDE02" );
			var outcome = sut.PlayerReport(forceIt:true);
			Console.WriteLine( "Report generated to {0}", outcome );
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestMattForte()
		{
			var sut = new NFLPlayer( "FORTMA01" );
			var outcome = sut.PlayerReport( true );
			Console.WriteLine( "Report generated to {0}", outcome );
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestTylerLocket()
		{
			var sut = new NFLPlayer( "LOCKTY01" );
			var outcome = sut.PlayerReport( true );
			Console.WriteLine( "Report generated to {0}", outcome );
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

        [TestMethod]
        public void TestAndyDalton()
        {
            var sut = new NFLPlayer("DALTAN02");
            var outcome = sut.PlayerReport(true);
            Console.WriteLine("Report generated to {0}", outcome);
            Assert.IsFalse(string.IsNullOrEmpty(outcome));
        }
    }
}