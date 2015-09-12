namespace RosterLib
{
	public class WinLossRecord
	{
		public int Wins { get; set; }
		public int Losses { get; set; }
		public int Ties { get; set; }

		public WinLossRecord( int wins, int losses, int ties )
		{
			Wins = wins;
			Losses = losses;
			Ties = ties;
		}

		public decimal Clip()
		{
			return Utility.Clip(Wins, Losses, Ties);
		}
	}
}
