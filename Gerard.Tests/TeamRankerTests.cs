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
			var sut = new RankingsJob( new FakeTimeKeeper( new DateTime( 2015, 10, 28 )) );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}
	}
}
