using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler.Models;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class SeasonProjectionTests
	{
		[TestMethod]
		public void TestDoSeasonProjectionJob()
		{
			var sut = new GameProjectionsJob(new TimeKeeper(null));
			var outcome = sut.DoJob();
			Assert.IsFalse(string.IsNullOrEmpty(outcome));
		}

		[TestMethod]
		public void TestNFLOutputMetric()
		{
			var t = new NflTeam( "CI" )
			{
				Season = "2016"
			};
			var predictor = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = false,
				WriteProjection = true,
				StorePrediction = true,
				RatingsService = new UnitRatingsService()
			};
			var sp = t.SeasonProjection( predictor, "Spread", new DateTime(2017,1,1));
			Assert.IsFalse( string.IsNullOrEmpty( sp ) );
		}

	}
}
