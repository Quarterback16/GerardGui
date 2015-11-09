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
			var sut = new RankingsJob( new FakeTimeKeeper( "2015", "08" ) );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}
	}
}
