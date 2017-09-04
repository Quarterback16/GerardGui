using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class UnitReportsTests
	{
		[TestMethod]
		public void TestUnitReportsJob()  //  2015-11-26  5 min : DEBUG, 133 min : DEBUG2
		{
			var sut = new UnitReportsJob( 
				new FakeTimeKeeper( season: "2017" ), 
				new FakeHistorian() );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestTimetoDoUnitReportsJob()
		{
			//  Fake historian garantees job will run always
			var sut = new UnitReportsJob( 
				new FakeTimeKeeper( season: "2017" ), 
				new FakeHistorian() );
			Assert.IsTrue( sut.IsTimeTodo( out string whyNot ) );
			Console.WriteLine( whyNot );
		}

		[TestMethod]
		public void TestDoUnitReportsJob()
		{
			//  Fake historian garantees job will run always
			var sut = new UnitReportsJob( new FakeTimeKeeper( season: "2017" ), new FakeHistorian() );
			var r = sut.DoJob();
			Assert.IsTrue( r.Length > 0 );
			Console.WriteLine( r );
			Console.WriteLine( " Runtime : {0}", sut.Report.RunTime );
		}

		[TestMethod]
		public void TestOpponent()
		{
			var g = new NFLGame( "2016:04-J" );
			var opp = g.Opponent( "DB" );
			Assert.AreEqual( "TB", opp );
		}

		[TestMethod]
		public void TestStarDefensiveBack()
		{
			var g = new NFLGame( "2016:04-J" );
			var star = g.StarDefensiveBack( "TB" );
			Assert.AreEqual( "DStewart", star );
		}

		public void TestUnitReportNiners()
		{
			//  Fake historian garantees job will run always
			var sut = new NflTeam( "SF" );
			var r = sut.PoReport();
			Assert.IsTrue( r.Length > 0 );
		}

		[TestMethod]
		public void TestOutputFileName()
		{
			//  Fake historian garantees job will run always
			var sut = new UnitReport( new FakeTimeKeeper( season: "2015" ) );
			var result = sut.OutputFilename();
			Console.WriteLine( result );
			Assert.IsFalse( string.IsNullOrEmpty( result ) );
			Assert.AreEqual( result, ".//Output//2015//Units" );
		}

		[TestMethod]
		public void TestUnitReportSD()
		{
			var sut = new UnitReport( new FakeTimeKeeper( season: "2015" ) )
			{
				SeasonMaster = Masters.Sm.GetSeason( "2015", teamsOnly: true )
			};
			sut.TeamUnits( "2015", "2015SD" );
		}
	}
}