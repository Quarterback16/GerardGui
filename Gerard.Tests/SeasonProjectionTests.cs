using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler.Models;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class SeasonProjectionTests
	{
		[TestMethod]
		public void TestDoSeasonProjectionJob()
		{
			var sut = new GameProjectionsJob(new TimeKeeper());
			var outcome = sut.DoJob();
			Assert.IsFalse(string.IsNullOrEmpty(outcome));
		}
	}
}
