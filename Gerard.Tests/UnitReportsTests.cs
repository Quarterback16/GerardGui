using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using Butler.Models;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class UnitReportsTests
   {
      [TestMethod]
      public void TestUnitReportsJob()  //  2015-11-26  5 min : DEBUG, 133 min : DEBUG2
      {
         var sut = new UnitReportsJob(new FakeHistorian());
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }


      [TestMethod]
      public void TestTimetoDoUnitReportsJob()
      {
         //  Fake historian garantees job will run always
         var sut = new UnitReportsJob( new FakeHistorian() );
         string whyNot;
         Assert.IsTrue( sut.IsTimeTodo( out whyNot ) );
         Console.WriteLine( whyNot );
      }


      [TestMethod]
      public void TestDoUnitReportsJob()
      {
         //  Fake historian garantees job will run always
         var sut = new UnitReportsJob( new FakeHistorian() );
         var r = sut.DoJob();
         Assert.IsTrue( r.Length > 0 );
         Console.WriteLine( r );
         Console.WriteLine( " Runtime : {0}", sut.Report.RunTime );
      }


		[TestMethod]
		public void TestOutputFileName()
		{
			//  Fake historian garantees job will run always
			var sut = new UnitReport();
			var result = sut.OutputFilename();
			Console.WriteLine(result);
			Assert.IsFalse( string.IsNullOrEmpty( result ) );
			Assert.AreEqual( result, ".//Output//2015//Units" );
		}

		[TestMethod]
		public void TestUnitReportSD()
		{
			var sut = new UnitReport {SeasonMaster = Masters.Sm.GetSeason( "2015", teamsOnly: true )};
			sut.TeamUnits( "2015", "2015SD" );
		}
   }
}
