using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class UnitPredictorTests
	{
		[TestMethod]
		public void TestUnitPredictorPredictGame()
		{
			var predictor = new UnitPredictor
			{
				TakeActuals = true,
				AuditTrail = true,
				WriteProjection = true,
				StorePrediction = false,
				RatingsService = new UnitRatingsService(
                    new FakeTimeKeeper() )
			};
			var game = new NFLGame( "2015:08-M" );  //  GB @ DB
			var result = predictor.PredictGame(
                game,
                new FakePredictionStorer(),
                new DateTime(2015,10,28) );
			Assert.IsTrue(
                result.HomeWin() );
			Assert.IsTrue(
                result.HomeScore.Equals( 6 ),
                $"Home score should be 6 not {result.HomeScore}" );
			Assert.IsTrue(
                result.AwayScore.Equals( 3 ),
                $"Away score should be 3 not {result.AwayScore}" );
		}

        [TestMethod]
        public void TestUnitPredictorPredict_IC_Game()
        {
            var predictor = new UnitPredictor
            {
                TakeActuals = true,
                AuditTrail = true,
                WriteProjection = false,
                StorePrediction = false,
                RatingsService = new UnitRatingsService(
                    new TimeKeeper(
                        clock:null))
            };
            var game = new NFLGame(
                "2020:01-B");  //  IC @ JJ  6.5 to the colts
            var result = predictor.PredictGame(
                game: game,
                persistor: new FakePredictionStorer(),
                predictionDate: new DateTime(2020, 08, 21));
            Assert.IsTrue(result.AwayWin());
            Assert.IsTrue(
                result.HomeScore.Equals(20),
                $"Home score should be 20 not {result.HomeScore}");
            Assert.IsTrue(
                result.AwayScore.Equals(24),
                $"Away score should be 24 not {result.AwayScore}");
        }

        [TestMethod]
        public void TestUnitPredictorPredict_OpenningGame()
        {
            var predictor = new UnitPredictor
            {
                TakeActuals = true,
                AuditTrail = true,
                WriteProjection = false,
                StorePrediction = false,
                RatingsService = new UnitRatingsService(
                    new TimeKeeper(
                        clock: null))
            };
            var game = new NFLGame(
                "2020:01-A");  //  HT @ KC  10 to the cheifs
            var result = predictor.PredictGame(
                game: game,
                persistor: new FakePredictionStorer(),
                predictionDate: new DateTime(2020, 08, 21));
            Assert.IsTrue(result.HomeWin());
            Assert.IsTrue(
                result.HomeScore.Equals(41),
                $"Home score should be 41 not {result.HomeScore}");
            Assert.IsTrue(
                result.AwayScore.Equals(31),
                $"Away score should be 31 not {result.AwayScore}");
        }
    }
}
