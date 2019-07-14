using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.RosterGridReports;
using System;
using System.Text;

namespace Gerard.Tests
{
	[TestClass]
	public class UpdateActualsJobTests
	{
		[TestMethod]
		public void TestUpdateActualsJob()
		{
			//  season starts at 2 as this is a prior week retrospective job
			var sut = new UpdateActualsJob(
                new FakeTimeKeeper( season: "2018", week: "02" ) );
			sut.DoJob();
			var run = sut.Report.LastRun;
			Assert.IsTrue( run.Date.Equals( DateTime.Now.Date ) );
		}

		[TestMethod]
		public void TestTimetoDoUpdateActuals()
		{
			var sut = new UpdateActualsJob(
                new FakeTimeKeeper( isPreSeason: true, isPeakTime: true ) );
			var result = sut.IsTimeTodo( out string whyNot );
			if ( !string.IsNullOrEmpty( whyNot ) )
				Console.WriteLine( whyNot );
			Assert.IsFalse( result );
		}

		[TestMethod]
		public void TestProcessOnePlayer()
		{
			var sut = new MetricsUpdateReport(
				new FakeTimeKeeper( season: "2017", week: "07" ) );
			var body = new StringBuilder();
			var p = new NFLPlayer( "HUNDBR01" );
			sut.ProcessPlayer( body, p );
			Console.WriteLine(body.ToString());
		}
	}
}