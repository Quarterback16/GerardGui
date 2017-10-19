﻿using RosterLib.Interfaces;
using System.Collections.Generic;
using System.Text;
using System;

namespace RosterLib.RosterGridReports
{
	public class MetricsUpdateReport : RosterGridReport
	{
		public NFLWeek Week { get; set; }

		public YahooScorer Scorer { get; set; }

		public IPlayerGameMetricsDao Dao { get; set; }

		public MetricsUpdateReport( IKeepTheTime timekeeper ) : base( timekeeper )
		{
			Name = "Metrics Update Report";
			Season = timekeeper.CurrentSeason();
			Week = new NFLWeek( Season, timekeeper.PreviousWeek() );
			Scorer = new YahooScorer( Week );
			Dao = new DbfPlayerGameMetricsDao();
		}

		public override string OutputFilename()
		{
			return $"{Utility.OutputDirectory()}{Season}/{Name}.htm";
		}

		public override void RenderAsHtml()
		{
			//TODO  process and add lines to a pre report
			var body = new StringBuilder();
			var gameList = Week.GameList();
			foreach ( NFLGame g in gameList )
			{
				body.AppendLine( g.GameName() + "  " + g.ScoreOut() );
				g.LoadAllFantasyAwayPlayers( null, string.Empty );
				g.LoadAllFantasyHomePlayers( null, string.Empty );
				ProcessPlayerList( g.HomePlayers, body );
				ProcessPlayerList( g.AwayPlayers, body );
#if DEBUG2
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

		private void ProcessPlayerList( IEnumerable<NFLPlayer> plist, StringBuilder body )
		{
			foreach ( var p in plist )
			{
				var pts = Scorer.RatePlayer( p, Week );
				p.Points = pts;
#if DEBUG
				if ( p.PlayerCode.Equals( "BRATCA01" ) )
					p.DumpMetrics();
#endif
				var line = $"   {p.PlayerNameShort,25} : {pts,2} > {p.ActualStats(),8}";
				if ( pts > 0 )
				{
					Announce( line );
					body.AppendLine( line );
				}
				p.UpdateActuals( Dao );
			}
		}

		private void Announce( string line )
		{
			Logger.Info( line );
			Console.WriteLine(line);
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