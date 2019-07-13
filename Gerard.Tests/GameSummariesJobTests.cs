using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;


namespace Gerard.Tests
{
	[TestClass]
	public class GameSummariesJobTests
	{
		[TestMethod]
		public void TestDoGameSummariesJob()
		{
			var sut = new GameSummariesJob(
                new FakeTimeKeeper(
                    season: "2017",
                    week: "01" ) );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestTimetoDoGameSummariesIsAfterWeek1()
		{
			var sut = new GameSummariesJob(
                new FakeTimeKeeper(
                    "2017",
                    "02" ) );
			Assert.IsTrue( sut.IsTimeTodo( out string whyNot ) );
			Console.WriteLine( whyNot );
		}

		[TestMethod]
		public void TestTimetoDoGameSummariesNotBeforeWeek2()
		{
			var sut = new GameSummariesJob(
                new FakeTimeKeeper(
                    "2017",
                    "01" ) );
			Assert.IsFalse( sut.IsTimeTodo( out string whyNot ) );
			Console.WriteLine( whyNot );
		}
	}
}
