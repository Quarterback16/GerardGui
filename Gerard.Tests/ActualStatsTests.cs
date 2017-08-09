using System;
using RosterLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
	[TestClass]
	public class ActualStatsTests
	{
		[Ignore]
		[TestMethod]
		public void TestActualStatsDerekCarrWeek10()
		{
			var p = new NFLPlayer("CARRDE01");
			var week = new NFLWeek("2015", "10");
			var Scorer = new YahooScorer(week);
			var pts = Scorer.RatePlayer(p, week);
			var result = p.ActualStats();
			Console.WriteLine( result );
			Assert.IsFalse(string.IsNullOrEmpty(result));
			Assert.AreEqual( expected:"302 (2)", actual:result);
			p.UpdateActuals(new DbfPlayerGameMetricsDao());
		}

		[Ignore]
		[TestMethod]
		public void TestActualStatsEifertWeek201509()
		{
			var p = new NFLPlayer("EIFETY01");
			var week = new NFLWeek("2015", "09");
			var Scorer = new YahooScorer(week);
			var pts = Scorer.RatePlayer(p, week);
			var result = p.ActualStats();
			Console.WriteLine(result);
			Assert.IsFalse(string.IsNullOrEmpty(result));
			Assert.AreEqual(expected: "53 (3)", actual: result);
		}

		[TestMethod]
		public void TestPlayerFantasyProjection()  //  5 sec 2015-08-11 
		{
			var game = new NFLGame("2015:10-L");
			game.GameWeek = new NFLWeek(game.Season, game.Week);
			var scorer = new YahooScorer(game.GameWeek);
			var p = new NFLPlayer("CARRDE01");
			var fpts = scorer.RatePlayer(p, game.GameWeek);
			Assert.AreEqual(fpts, 19);
		}
	}
}
