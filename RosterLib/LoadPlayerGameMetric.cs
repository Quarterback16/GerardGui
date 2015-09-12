namespace RosterLib
{
   /// <summary>
   ///   This is a "Filter"
   /// </summary>
   public class LoadPlayerGameMetric
   {
      public LoadPlayerGameMetric( YahooProjectedPointsMessage input )
      {
         if ( input.Game != null )
            Process( input, new DbfPlayerGameMetricsDao() );
      }

      private static void Process( YahooProjectedPointsMessage input, IPlayerGameMetricsDao dao )
      {
         input.PlayerGameMetrics = dao.Get( input.Player.PlayerCode, input.Game.GameKey() );
      }
   }
}