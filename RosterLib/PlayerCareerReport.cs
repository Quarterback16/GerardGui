using RosterLib.Interfaces;
using System;

namespace RosterLib
{
	public class PlayerCareerReport : RosterGridReport
	{
		public NFLRosterReport RosterReport { get; set; }

		public PlayerCareerReport( IKeepTheTime timekeeper ) : base( timekeeper )
		{
			Season = timekeeper.CurrentSeason( DateTime.Now );
		}

		public override void RenderAsHtml()
		{
			Name = "Career Reports";
			RosterReport = new NFLRosterReport( Season );
			RosterReport.LoadAfc();
			RosterReport.PlayerReports();
		}

		public override string OutputFilename()
		{
			var fileName = $"{Utility.OutputDirectory()}\\Players\\Errors.htm";
			return fileName;
		}
	}
}