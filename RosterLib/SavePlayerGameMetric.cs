
namespace RosterLib
{
   //  Filter
   public class SavePlayerGameMetrics
   {
      public SavePlayerGameMetrics( PlayerGameProjectionMessage input )
      {
         Process( input );
      }

      private static void Process( PlayerGameProjectionMessage input )
      {
         var nMetrics = 0;
         foreach ( var pgm in input.Game.PlayerGameMetrics )
         {
            pgm.Save( input.Dao );
            nMetrics++;
         }
#if DEBUG
         Utility.Announce( string.Format( "Metrics saved {0} for {1} to {2}", 
            nMetrics, input.Game, Utility.TflWs.NflConnectionString ) );
#endif
      }

   }
}
