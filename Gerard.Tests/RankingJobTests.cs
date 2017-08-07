using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;
using System.IO;

namespace Gerard.Tests
{
	[TestClass]
	public class RankingJobTests
	{
		[TestMethod]
		public void TestRankingJob()
		{
			var sut = new RankingsJob( new FakeTimeKeeper( new DateTime( 2017, 8, 3) ) );
			sut.DoJob();
			var fileOut = sut.TeamRanker.FileOut;
			Assert.IsTrue( File.Exists( fileOut ), $"Cannot find {fileOut}" );
		}

		[TestMethod]
		public void TestInterceptionRatio()
		{
			var ratio = TeamRanker.InterceptionRatio( interceptions:12, touchDownPassesAllowed: 19 );
			Assert.IsTrue( ratio ==  0.63M );
		}
	}
}
