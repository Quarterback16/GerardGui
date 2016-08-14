using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class PullMetricsFromPredictionTests
   {
      [TestMethod]
      public void TestJayCutler()
      {
         var g = new NFLGame( "2016:01-I" );
         var msg = new PlayerGameProjectionMessage();
         msg.Player = new NFLPlayer( "CUTLJA01" );
         msg.Game = g;
         msg.Prediction = g.GetPrediction( "unit" );
         var cut = new PullMetricsFromPrediction( msg );
         Assert.IsNotNull( msg.Game.PlayerGameMetrics.Count > 12 );
         msg.Dao = new DbfPlayerGameMetricsDao();
         var saveStep = new SavePlayerGameMetrics( msg );

      }
   }
}
