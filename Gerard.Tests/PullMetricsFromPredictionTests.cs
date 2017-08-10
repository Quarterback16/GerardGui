using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class PullMetricsFromPredictionTests
	{
		[TestMethod]
		public void TestPullMetricsFromPredictionProcess()
		{
			//  Processing happens in the constructor, bit smelly
			var g = new FakeNFLGame();
			var msg = new PlayerGameProjectionMessage()
			{
				Player = new FakeNFLPlayer( "", "", "Unkown soldier" ),
				Game = g,
				Prediction = g.GetPrediction( "unit" )
			};
			var sut = new PullMetricsFromPrediction( msg );
			Assert.IsTrue( msg.Game.PlayerGameMetrics.Count > 0 );
		}


		[TestMethod]
		public void TestJayCutler()
		{
			var g = new NFLGame( "2016:01-I" );
			var msg = new PlayerGameProjectionMessage()
			{
				Player = new NFLPlayer( "CUTLJA01" ),
				Game = g,
				Prediction = g.GetPrediction( "unit" )
			};
			var cut = new PullMetricsFromPrediction( msg );
			Assert.IsNotNull( msg.Game.PlayerGameMetrics.Count > 12 );
			msg.Dao = new DbfPlayerGameMetricsDao();
			var saveStep = new SavePlayerGameMetrics( msg );
		}
	}
}