using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using Butler.Models;

namespace Gerard.Tests
{
	[TestClass]
	public class GoalLineReportJobTests
	{
		[TestMethod]
		public void TestDoGoalLineReportJob()  //  
		{
			var sut = new GoalLineReportJob( new TimeKeeper( null ) );
			var outcome = sut.Execute();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestDoGoalLineReportJobWeek01()  //  
		{
			var sut = new GoalLineReportJob( 
				new FakeTimeKeeper(
					season: "2017",
					week : "01" ) 
				);
			var outcome = sut.Execute();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}
	}
}
