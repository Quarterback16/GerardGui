using System.Collections.Generic;

namespace RosterLib
{
	public class TeamCards : RosterGridReport
	{
		public string LastOutput { get; set; }

		public NFLRosterReport RosterReport { get; set; }

		public List<RosterGridLeague> Leagues { get; set; }

		public bool DoPlayerReports { get; set; }

		public TeamCards( bool doPlayerReports )
		{
			Name = "Team Cards";
			RosterReport = new NFLRosterReport(Season) {Season = Season};
		   DoPlayerReports = doPlayerReports;
		   Leagues = new List<RosterGridLeague>
		   {
		      new RosterGridLeague {Id = Constants.K_LEAGUE_Gridstats_NFL1, Name = "Gridstats GS1"}
		   };
		}

		public override void RenderAsHtml()
		{
			foreach (var league in Leagues)
			{
				RosterReport.TeamCards();
			}
			if (DoPlayerReports)
				RosterReport.PlayerReports();
		}

		public override string OutputFilename()
		{
			var fileOut = string.Format("{0}{2}/TeamCards/TeamCard_{1}.htm", Utility.OutputDirectory(), "SF", Season);
			return fileOut;
		}
	}
}