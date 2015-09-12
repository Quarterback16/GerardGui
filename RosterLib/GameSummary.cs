using System;
using System.Data;

namespace RosterLib
{
	/// <summary>
	///    Result of a game basically showing useful information
	/// </summary>
	public class GameSummary
	{
		public NFLGame Game { get; set; }

		public GameSummary( NFLGame game )
		{
			Game = game;
			Game.LoadLineups();
		}

		public string FileName()
		{
			var fileName = Game.SummaryFile();
			return fileName;
		}

		public void Render()
		{
			//  might have to load some stuff first ???

			var str = new SimpleTableReport( "Game Summary " + Game.ScoreOut() );
			str.AddDenisStyle();
			str.SubHeader = SubHeading();
			str.AnnounceIt = true;
			str.AddColumn( new ReportColumn( "C1", "COL01", "{0}" ) );
			str.AddColumn( new ReportColumn( "C2", "COL02", "{0}" ) );
			str.AddColumn( new ReportColumn( "C3", "COL03", "{0}" ) );
			str.AddColumn( new ReportColumn( "C4", "COL04", "{0}" ) );
			str.AddColumn( new ReportColumn( "C5", "COL05", "{0}" ) );
			str.AddColumn( new ReportColumn( "C6", "COL06", "{0}" ) );
			str.AddColumn( new ReportColumn( "C7", "COL07", "{0}" ) );
			str.AddColumn( new ReportColumn( "C8", "COL08", "{0}" ) );
			str.AddColumn( new ReportColumn( "C9", "COL09", "{0}" ) );
			str.AddColumn( new ReportColumn( "C10", "COL10", "{0}" ) );
			str.AddColumn( new ReportColumn( "C11", "COL11", "{0}" ) );

			str.CustomHeader = SummaryHeader();

			str.LoadBody( BuildTable() );
			str.RenderAsHtml( FileName(), persist: true );
		}

		private static string SummaryHeader()
		{
			var htmlOut =
				HtmlLib.TableRowOpen() + "\n\t\t"
				+ HtmlLib.TableDataAttr( HtmlLib.Bold( "AWAY" ), "colspan='5' class='gponame'" ) + "\n\t"
				+ HtmlLib.TableDataAttr( HtmlLib.Bold( "---" ), "colspan='1' class='gponame'" ) + "\n\t"
				+ HtmlLib.TableDataAttr( HtmlLib.Bold( "HOME" ), "colspan='5' class='gponame'" ) + "\n\t"
				+ HtmlLib.TableRowClose() + "\n";
			return htmlOut;
		}

		private DataTable BuildTable()
		{
			var dt = new DataTable();
			var cols = dt.Columns;
			cols.Add( "COL01", typeof (String) );
			cols.Add( "COL02", typeof (String) );
			cols.Add( "COL03", typeof (String) );
			cols.Add( "COL04", typeof (String) );
			cols.Add( "COL05", typeof (String) );
			cols.Add( "COL06", typeof (String) );
			cols.Add( "COL07", typeof (String) );
			cols.Add( "COL08", typeof (String) );
			cols.Add( "COL09", typeof (String) );
			cols.Add( "COL10", typeof (String) );
			cols.Add( "COL11", typeof (String) );

			AddTDrRow( dt );
			AddTDpRow( dt );
			AddTDdRow( dt );
			AddTDsRow( dt );
			AddFGsRow( dt );
			AddYDrRow( dt );
			AddYDpRow( dt );

			AddQB1Row( dt );

			AddRB1Row( dt );

			return dt;
		}


		private void AddRB1Row( DataTable dt )
		{
			var dr = dt.NewRow();
			dr[ "COL01" ] = Game.AwayRb1.PlayerName;
			dr[ "COL02" ] = Game.AwayRb1.CurrentGameMetrics.YDp;
			dr[ "COL03" ] = Game.AwayRb1.CurrentGameMetrics.TDp;
			dr[ "COL04" ] = Game.AwayRb1.CurrentGameMetrics.YDr;
			dr[ "COL05" ] = Game.AwayRb1.CurrentGameMetrics.TDr;
			dr[ "COL06" ] = "RB";
			dr[ "COL07" ] = Game.HomeRb1.PlayerName;
			dr[ "COL08" ] = Game.HomeRb1.CurrentGameMetrics.YDp;
			dr[ "COL09" ] = Game.HomeRb1.CurrentGameMetrics.TDp;
			dr[ "COL10" ] = Game.HomeRb1.CurrentGameMetrics.YDr;
			dr[ "COL11" ] = Game.HomeRb1.CurrentGameMetrics.TDr;
			dt.Rows.Add( dr );
		}

		private void AddQB1Row( DataTable dt )
		{
			var dr = dt.NewRow();
			dr[ "COL01" ] = Game.AwayQb1.PlayerName;
			dr[ "COL02" ] = Game.AwayQb1.CurrentGameMetrics.YDp;
			dr[ "COL03" ] = Game.AwayQb1.CurrentGameMetrics.TDp;
			dr[ "COL04" ] = Game.AwayQb1.CurrentGameMetrics.YDr;
			dr[ "COL05" ] = Game.AwayQb1.CurrentGameMetrics.TDr;
			dr[ "COL06" ] = "QB";
			dr[ "COL07" ] = Game.HomeQb1.PlayerName;
			dr[ "COL08" ] = Game.HomeQb1.CurrentGameMetrics.YDp;
			dr[ "COL09" ] = Game.HomeQb1.CurrentGameMetrics.TDp;
			dr[ "COL10" ] = Game.HomeQb1.CurrentGameMetrics.YDr;
			dr[ "COL11" ] = Game.HomeQb1.CurrentGameMetrics.TDr;
			dt.Rows.Add( dr );
		}

		private void AddYDpRow( DataTable dt )
		{
			var dr = dt.NewRow();
			dr[ "COL04" ] = Game.AwayYDp;
			dr[ "COL06" ] = "YDp";
			dr[ "COL08" ] = Game.HomeYDp;
			dt.Rows.Add( dr );
		}

		private void AddYDrRow( DataTable dt )
		{
			var dr = dt.NewRow();
			dr[ "COL04" ] = Game.AwayYDr;
			dr[ "COL06" ] = "YDr";
			dr[ "COL08" ] = Game.HomeYDr;
			dt.Rows.Add( dr );
		}

		private void AddTDrRow( DataTable dt )
		{
			var dr = dt.NewRow();
			dr[ "COL04" ] = Game.AwayTDr;
			dr[ "COL06" ] = "TDr";
			dr[ "COL08" ] = Game.HomeTDr;
			dt.Rows.Add( dr );
		}

		private void AddTDpRow( DataTable dt )
		{
			var dr = dt.NewRow();
			dr[ "COL04" ] = Game.AwayTDp;
			dr[ "COL06" ] = "TDp";
			dr[ "COL08" ] = Game.HomeTDp;
			dt.Rows.Add( dr );
		}

		private void AddTDdRow( DataTable dt )
		{
			var dr = dt.NewRow();
			dr[ "COL04" ] = Game.AwayTDd;
			dr[ "COL06" ] = "TDd";
			dr[ "COL08" ] = Game.HomeTDd;
			dt.Rows.Add( dr );
		}

		private void AddTDsRow( DataTable dt )
		{
			var dr = dt.NewRow();
			dr[ "COL04" ] = Game.AwayTDs;
			dr[ "COL06" ] = "TDs";
			dr[ "COL08" ] = Game.HomeTDs;
			dt.Rows.Add( dr );
		}

		private void AddFGsRow( DataTable dt )
		{
			var dr = dt.NewRow();
			dr[ "COL04" ] = Game.AwayFg;
			dr[ "COL06" ] = "FGs";
			dr[ "COL08" ] = Game.HomeFg;
			dt.Rows.Add( dr );
		}

		private string SubHeading()
		{
			var header = Legend();
			var div = HtmlLib.DivOpen( "id=\"main\"" ) + GameData() + EndDiv() + HtmlLib.DivClose();
			return string.Format( "{0}{1}\n", header, div );
		}

		private string Legend()
		{
			return string.Format( "\n<h3>{0}</h3>\n", Game.ScoreOut() );
		}

		private static string EndDiv()
		{
			return HtmlLib.DivOpen( "class=\"end\"" ) + HtmlLib.Spaces( 1 ) + HtmlLib.DivClose() + "\n";
		}

		private string GameData()
		{
			var s = String.Empty;
			s += DataOut( "Date", Game.GameDate.ToLongDateString() );
			s += DataOut( "Code", Game.GameKey() );
			s += DataOut( "Divisional", Game.IsDivisionalGame() ? "Yes" : "No" );
			s += DataOut( "Bad Weather", Game.IsBadWeather() ? "Yes" : "No" );
			s += DataOut( "Dome", Game.IsDomeGame() ? "Yes" : "No" );
			s += DataOut( "Monday Night", Game.IsMondayNight() ? "Yes" : "No" );
			s += DataOut( "TV", Game.IsOnTv ? "Yes" : "No" );
			s += DataOut( "Spread", string.Format( "{0:0.0}", Game.Spread ) );
			return s;
		}

		private static string DataOut( string label, string val )
		{
			return string.Format( "<label>{0}:</label> <value>{1,8}</value>", label, val );
		}
	}
}