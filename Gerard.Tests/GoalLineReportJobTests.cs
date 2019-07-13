using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using Butler.Models;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class GoalLineReportJobTests
	{
		[TestMethod]
		public void TestDoGoalLineReportJob_ForaSeason()  
		{
            var sut = new GoalLineReportJob(
                new FakeTimeKeeper(
                    season: "2018",
                    week: null ));

            var outcome = sut.Execute();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

        [TestMethod]
        public void TestDoGoalLineReportJob_ForaWeek()
        {
            var sut = new GoalLineReportJob(
                new FakeTimeKeeper(
                    season: "2018",
                    week: "01"));

            var outcome = sut.Execute();
            Assert.IsFalse(string.IsNullOrEmpty(outcome));
        }

        [TestMethod]
		public void TimeToDoGoalLineReport_WhenItsWeek1_ReturnsFalse()
        {
			var sut = new GoalLineReportJob( 
				new FakeTimeKeeper(
					season: "2017",
					week : "01" ) 
				);
			var outcome = sut.Execute();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

        [TestMethod]
        public void TimeToDoGoalLineReport_WhenIsTuesday_ReturnsFalse()
        {
            var sut = new GoalLineReportJob(
                new FakeTimeKeeper(
                    season: "2019",
                    week: "01",
                    new DateTime(2019,07,09))); // a Tuesday
            var result = sut.IsTimeTodo(out string outcome);
            Assert.IsFalse(result);
            Assert.AreEqual(
                expected: outcome,
                actual: "Not on Tuesdays");
        }

        [TestMethod]
        public void TimeToDoGoalLineReport_WhenIsPeakTime_ReturnsFalse()
        {
            var sut = new GoalLineReportJob(
                new FakeTimeKeeper(
                    isPreSeason: false,
                    isPeakTime: true)); 
            var result = sut.IsTimeTodo(out string outcome);
            Assert.IsFalse(result);
            Assert.AreEqual(
                expected: outcome,
                actual: "Peak time - no noise please");
        }

        [TestMethod]
        public void TimeToDoGoalLineReport_WhenSeasonHasntStarted_ReturnsFalse()
        {
            var sut = new GoalLineReportJob(
                new FakeTimeKeeper(
                    isPreSeason: true,
                    isPeakTime: true));
            var result = sut.IsTimeTodo(out string outcome);
            Assert.IsFalse(result);
            Assert.AreEqual(
                expected: outcome,
                actual: "The Season hasnt started yet");
        }
    }
}
