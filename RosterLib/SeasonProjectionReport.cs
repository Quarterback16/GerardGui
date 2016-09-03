using System;

namespace RosterLib
{
	public class SeasonProjectionReport : RosterGridReport
	{
		public NFLRosterReport RosterReport { get; set; }

		public string MetricName { get; set; }
		public string Week { get; set; }

		public SeasonProjectionReport()
		{
			// push initialisation out of construtor
		}

      public SeasonProjectionReport( string season, string week )
      {
         Season = season;
         Week = week;
      }

		public override void RenderAsHtml()
		{
			Name = "Season Projections";
			RosterReport = new NFLRosterReport(Season);
			MetricName = "Spread";
			RosterReport.SeasonProjection( MetricName, Season, Week, DateTime.Now );
         SetLastRunDate();
      }

		public override string OutputFilename()
		{
			var fileName =
				string.Format( "{0}{2}\\Projections\\Proj-{1}-{2}.htm",
									Utility.OutputDirectory(), MetricName,
									Season );
			return fileName;
		}
	}
}