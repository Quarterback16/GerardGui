using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
	[TestClass]
	public class TeamRankerTests
	{
		[TestMethod]
		public void TestTeamRankerJob()
		{
			var sut = new RankingsJob( new FakeTimeKeeper( DateTime.Now ) );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}
	}
}
