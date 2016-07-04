using System;
using System.Data;

namespace RosterLib
{
   /// <summary>
   ///   PlayerReport outputs a sheet of metrics on a player.
   /// </summary>
   public class PlayerReport
	{
		public PlayerReport( string playerId )
		{
			Player = new NFLPlayer( playerId );
		}

		public PlayerReport( NFLPlayer player )
		{
			Player = player;
		}

		public NFLPlayer Player { get; set; }

		/// <summary>
		///   Creates the output.
		/// </summary>
		public void Render()
		{
			var str = new SimpleTableReport( "Player Profile " + Player.PlayerName );
			str.AddStyle(  "#container { text-align: left; background-color: #ccc; margin: 0 auto; border: 1px solid #545454; width: 641px; padding:10px; font: 13px/19px Trebuchet MS, Georgia, Times New Roman, serif; }" );
			str.AddStyle(  "#main { margin-left:1em; }" );
			str.AddStyle(  "#dtStamp { font-size:0.8em; }" );
			str.AddStyle(  ".end { clear: both; }" );
			str.AddStyle(  ".gponame { color:white; background:black }" );
			str.AddStyle(  "label { display:block; float:left; width:130px; padding: 3px 5px; margin: 0px 0px 5px 0px; text-align:right; }" );
			str.AddStyle(  "value { display:block; float:left; width:100px; padding: 3px 5px; margin: 0px 0px 5px 0px; text-align:left; font-weight: bold; color:blue }" );
			str.AddStyle(  "#notes { float:right; height:auto; width:308px; font-size: 88%; background-color: #ffffe1; border: 1px solid #666666; padding: 5px; margin: 0px 0px 10px 10px; color:#666666 }" );
			str.AddStyle(  "div.notes H4 { background-image: url(images/icon_info.gif); background-repeat: no-repeat; background-position: top left; padding: 3px 0px 3px 27px; border-width: 0px 0px 1px 0px; border-style: solid; border-color: #666666; color: #666666; font-size: 110%;}" );
			str.ColumnHeadings = true;
			str.DoRowNumbers = true;
			str.ShowElapsedTime = false;
			str.IsFooter = false;
			str.AddColumn( new ReportColumn( "Week",      "WEEK",   "{0}"           ) ); 
			str.AddColumn( new ReportColumn( "Team",      "TEAM",   "{0}"           ) ); 
			str.AddColumn( new ReportColumn( "U-Res",     "URES",   "{0}"           ) ); 
			str.AddColumn( new ReportColumn( "EP",        "EP",     "{0:0.0}", true ) ); 
			str.AddColumn( new ReportColumn( "Matchup",   "MATCH",  "{0}"           ) ); 
			str.AddColumn( new ReportColumn( "Stats",     "STATS",  "{0}"           ) );
         str.AddColumn( new ReportColumn( "F Pts",     "FPTS",   "{0}", true ) );
         str.LoadBody( BuildTable() );
			str.SubHeader = SubHeading();
            str.RenderAsHtml( string.Format("{0}players//{1}.htm", 
					Utility.OutputDirectory(), Player.PlayerCode), true);
		}
			
		private DataTable BuildTable()
		{
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add( "WEEK",      typeof( String ) );
			cols.Add( "TEAM",      typeof( String ) );
			cols.Add( "URES",      typeof( String ) );
			cols.Add( "EP",        typeof( Decimal ) );
			cols.Add( "MATCH",     typeof( String ) );
			cols.Add( "STATS",     typeof( String ) );
         cols.Add( "FPTS",      typeof( Decimal ) );

         if (Player.PerformanceList == null) 
				Player.LoadPerformances( true, false, Utility.CurrentSeason()); //  all games=true, coz we want to see the career

		   if (Player.PerformanceList == null) return dt;

		   foreach ( NflPerformance g in Player.PerformanceList )
		   {
		      if (g.Game == null) continue;
		      var week = new NFLWeek( g.Season, g.Week );
		      var scorer = new YahooScorer( week );
		      var dr = dt.NewRow();
		      g.Game.TallyMetrics(String.Empty);
		      dr[ "WEEK" ]  = g.Game.GameCodeOut();
		      dr[ "TEAM" ]  = string.Format( "{0}-{1}", g.TeamCode, Player.Unit() );
		      dr[ "URES" ] = g.Game.UnitResult( Player.Unit(), g.TeamCode );
		      dr[ "EP" ] = g.Game.ExperiencePoints( Player, g.TeamCode );							
		      dr[ "MATCH" ] = g.Game.ScoreOut3();
		      dr[ "STATS" ] = g.PerfStats.Stat1( Player.PlayerCat, false );
		      dr[ "FPTS" ] = Player.PointsForWeek( week, scorer );
		      dt.Rows.Add( dr );
		   }
		   //  save the table for Ron?
			return dt;
		}
		
		private string SubHeading()
		{
			var header = Legend(); 
			var div = HtmlLib.DivOpen( "id=\"main\"" ) + PlayerData() + EndDiv() +  HtmlLib.DivClose();
			return string.Format( "{0}{1}\n", header, div );
		}
		
		private string Legend()
		{
			var status = Player.Status();
			var lastSeason = "";
			if ( Player.IsRetired ) lastSeason = Player.LastSeason;
			return string.Format( "\n<h3>{0} - {1} {2}</h3>\n", Player.PlayerName, status, lastSeason );
		}
		
		private static string EndDiv()
		{
			return HtmlLib.DivOpen( "class=\"end\"" ) + HtmlLib.Spaces( 1 ) + HtmlLib.DivClose() + "\n";
		}
		
		private string PlayerData()
		{
			var s = PlayerNotes();
			Player.SetDraftRound();
            Player.CalculateEp( Utility.CurrentSeason() );
			s += DataOut( "Position", Player.PlayerPos );
			s += DataOut( "Rookie Yr", Player.RookieYear );
			s += DataOut( "Acquired", Player.Drafted );
			s += DataOut( "Seasons", Player.Seasons() );
			s += DataOut( "Scores per yr", String.Format( "{0:##.#}", Player.ScoresPerYear() ) );
			s += DataOut( "Age", Player.PlayerAge() );
			s += DataOut( "Current Value", Player.Value().ToString() );
			s += DataOut( "Experience Pts", Player.ExperiencePoints.ToString() );
			return s;
		}
		
		private string PlayerNotes()
		{
			//string s = HtmlLib.H4( "Notes", "style=\"outline-color: rgb(0, 0, 255); outline-style: solid; outline-width: 1px;\"" );
			var s = HtmlLib.DivOpen( "id=\"notes\"" ) + string.Format( "<p>\n{0}</p>\n", Player.Bio ) + HtmlLib.DivClose();
			return s;
		}
		
		private static string DataOut( string label, string val )
		{
			return string.Format( "<label>{0}:</label> <value>{1,8}</value>", label, val );
		}
	}
}
