namespace RosterLib
{
	public class AddYahooKickingPoints
	{
		public AddYahooKickingPoints(YahooProjectedPointsMessage input)
      {
#if DEBUG
         Utility.Announce( string.Format( "Calculating Kicking Points for {0} Game {1}",
            input.Player.PlayerNameShort, input.Game.GameName() ) );
#endif
         Process( input );
      }

		private static void Process(YahooProjectedPointsMessage input)
		{
			input.Player.Points += input.PlayerGameMetrics.ProjFG * 3;
#if DEBUG
			Utility.Announce(string.Format("Projected FG = {0} * 3 = {1}",
				input.PlayerGameMetrics.ProjFG, input.PlayerGameMetrics.ProjFG * 3));
#endif
			input.Player.Points += input.PlayerGameMetrics.ProjPat * 1;
#if DEBUG
			Utility.Announce(string.Format("Projected Pat = {0} * 1 = {1}",
				input.PlayerGameMetrics.ProjPat, input.PlayerGameMetrics.ProjPat * 1));
#endif
#if DEBUG
			Utility.Announce(string.Format("Projected FP = {0}", input.Player.Points));
#endif
		}
	}
}
