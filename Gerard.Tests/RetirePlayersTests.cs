using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using Butler.Models;

namespace Gerard.Tests
{
	[TestClass]
	public class RetirePlayersTests
	{
		[TestMethod]
		public void TestRetirePlayersJob()
		{
			var sut = new RetirePlayersJob( new TimeKeeper( null ) );
			sut.DoJob();
			var run = sut.Report.LastRun;
			Console.WriteLine( "Last Run : {0}", run );
			Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
		}

		[TestMethod]
		public void TestTimetoRetirePlayers()
		{
			// need to remove the file if it is there (bad test relies on environment)
			var sut = new RetirePlayersJob( new FakeTimeKeeper( isPreSeason: true, isPeakTime: false ) );
			var isTime = sut.IsTimeTodo( out string whyNot );
			Console.WriteLine( whyNot );
			Assert.IsTrue( isTime );
		}

		[TestMethod]
		public void TestSingleProbableRetire()
		{
			var sut = new NFLPlayer( "MOSSSA01" );

			Assert.IsTrue( sut.IsProbablyRetired(2017) );
		}

		[TestMethod]
		public void TestSingleRetire()
		{
			var sut = new NFLPlayer( "BRYADE02" );
			sut.Retire();
			Assert.IsTrue( sut.IsRetired );
		}

	}
}
