using System.Collections.Generic;
using System.Text;
using RosterLib.Interfaces;

namespace RosterLib.RosterGridReports
{
   public class MetricsUpdateReport : RosterGridReport
   {
      public NFLWeek Week { get; set; }

	   public YahooScorer Scorer { get; set; }

		public IPlayerGameMetricsDao Dao { get; set; }

      public MetricsUpdateReport( IKeepTheTime timekeeper)
      {
         Name = "Metrics Update Report";
         Season = timekeeper.CurrentSeason();
	      Week = new NFLWeek( Season, timekeeper.PreviousWeek());
			Scorer = new YahooScorer(Week);
			Dao = new DbfPlayerGameMetricsDao();
      }

      public override string OutputFilename()
      {
         return string.Format( "{0}{1}/{2}.htm", Utility.OutputDirectory(), Season, Name );
      }

      public override void RenderAsHtml()
      {
         //TODO  process and add lines to a pre report
	      var body = new StringBuilder();
	      var gameList = Week.GameList();
	      foreach (NFLGame g in gameList)
	      {
		      body.AppendLine( g.GameName() + "  " + g.ScoreOut() );
		      g.LoadAllFantasyAwayPlayers();
				g.LoadAllFantasyHomePlayers();
		      ProcessPlayerList(g.HomePlayers, body);
				ProcessPlayerList(g.AwayPlayers, body);
#if DEBUG
		      break;  // to speed things up
#endif
	      }
	      //For each game in the last week
			//  for each player
			//     get actuals
			//     save them
         OutputReport( body.ToString() );
         Finish();
      }

	   private void ProcessPlayerList(IEnumerable<NFLPlayer> plist, StringBuilder body)
	   {
		   foreach (var p in plist)
		   {
			   var pts = Scorer.RatePlayer(p, Week);
				//p.DumpMetrics();
				if ( pts > 0 )
					body.AppendLine(string.Format("   {0,25} : {1,2} > {2,8}", 
						p.PlayerNameShort, pts, p.ActualStats()) );
			   p.UpdateActuals(Dao);
		   }
	   }

	   private void OutputReport( string body )
      {
         var PreReport = new SimplePreReport
         {
            ReportType = Name,
            Folder = "Metrics",
            Season = Season,
            InstanceName = string.Format( "MetricsUpdates-{0:0#}", Week ),
            Body = body
         };
         PreReport.RenderHtml();
         FileOut = PreReport.FileOut;
      }
   }
}