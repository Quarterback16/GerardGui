using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler.Models;

namespace Gerard.Tests
{
	[TestClass]
	public class SeasonProjectionTests
	{
		[TestMethod]
		public void TestDoSeasonProjectionJob()
		{
			var sut = new ProjectionsJob(new FakeTimeKeeper());
			var outcome = sut.DoJob();
			Assert.IsFalse(string.IsNullOrEmpty(outcome));
		}
	}
}
