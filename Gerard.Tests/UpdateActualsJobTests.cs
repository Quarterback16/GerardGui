using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class UpdateActualsJobTests
	{
		[TestMethod]
		public void TestUpdateActualsJob()
		{
         //  season starts at 2 as this is a prior week retrospective job
			var sut = new UpdateActualsJob(new FakeTimeKeeper( season:"2016", week:"12" ));
			sut.DoJob();
			var run = sut.Report.LastRun;
			Assert.IsTrue(run.Date.Equals(DateTime.Now.Date));
		}

      [TestMethod]
      public void TestTimetoDoUpdateActuals()
      {
         var sut = new UpdateActualsJob( new FakeTimeKeeper( isPreSeason: true, isPeakTime: true ) );
         string whyNot;
         var result = sut.IsTimeTodo( out whyNot );
         if ( !string.IsNullOrEmpty( whyNot ) )
            Console.WriteLine( whyNot );
         Assert.IsFalse( result );
      }

   }
}