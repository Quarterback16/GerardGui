using System;


namespace RosterLib
{
   /// <summary>
   ///  filter for calculating yahoo points
   /// </summary>
   public class AddYahooPassingPoints
   {
      public AddYahooPassingPoints( YahooProjectedPointsMessage input )
      {
#if DEBUG
         Utility.Announce(string.Format("Calculating Passing Points for {0} Game {1}", 
            input.Player.PlayerNameShort, input.Game.GameName() ) );
#endif
         Process( input );
      }

      private static void Process( YahooProjectedPointsMessage input )
      {
			input.Player.Points += input.PlayerGameMetrics.ProjTDp * 4;
#if DEBUG
         Utility.Announce(string.Format("Projected TDp = {0} * 4 = {1}", input.PlayerGameMetrics.ProjTDp, input.PlayerGameMetrics.ProjTDp * 4));
#endif
         var yardagePts = Math.Floor( (decimal) input.PlayerGameMetrics.ProjYDp / 25 );
#if DEBUG
         Utility.Announce(string.Format("Projected YDp = {0} / 25 = {1}", input.PlayerGameMetrics.ProjYDp, input.PlayerGameMetrics.ProjYDp / 25 ));
#endif
         input.Player.Points += yardagePts;
         //TODO:  -1 for Interceptions
         //TODO:  +2 per PAT pass
#if DEBUG
         Utility.Announce(string.Format("Projected FP = {0}", input.Player.Points ));
#endif
      }

   }
}
