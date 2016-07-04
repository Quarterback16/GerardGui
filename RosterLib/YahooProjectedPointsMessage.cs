namespace RosterLib
{
   public class YahooProjectedPointsMessage
	{
		public NFLPlayer Player { get; set; }
		public NFLGame Game { get; set; }
		public PlayerGameMetrics PlayerGameMetrics { get; set; }
	}
}
