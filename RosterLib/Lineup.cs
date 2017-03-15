using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RosterLib
{
	public class Lineup
	{
		public string TeamCode { get; set; }
		public int MissingKeys { get; set; }

		public List<NFLPlayer> PlayerList;

		public Lineup(DataSet ds)
		{
			LoadPlayerList(ds);
			MissingKeys = 0;
		}

		public Lineup(string teamCode, string seasonIn, string week)
		{
			TeamCode = teamCode;
			var ds = Utility.TflWs.GetLineup(teamCode, seasonIn, Int32.Parse(week));
			LoadPlayerList(ds);
			MissingKeys = 0;
		}

		public void DumpLineup()
		{
			DumpOffence();
			DumpDefence();
		}

      public string DumpAsHtml(string header)
      {
         var sb = new StringBuilder();
         sb.Append( HtmlLib.H3( header ) );
         sb.Append( HtmlLib.TableOpen( "border='0'" ) );
         sb.Append( HtmlLib.TableRowOpen() );
         sb.Append( HtmlLib.TableData( DumpOffenceHtml() ) );
         sb.Append( HtmlLib.TableData( DumpDefenceHtml() ) );
         sb.Append( HtmlLib.TableRowClose() );
         sb.Append( HtmlLib.TableClose() );

         return sb.ToString();
      }

      private string DumpDefenceHtml()
      {
         var sb = new StringBuilder();
         sb.Append( HtmlLib.ListOpen() );
         foreach ( var p in PlayerList )
         {
            if ( p.LineupPos.Trim().Length > 0 )
               if ( p.IsDefence() )
                  sb.Append( HtmlLib.ListItem( $"{p.LineupPos} {p.PlayerName}" ) );
         }
         sb.Append( HtmlLib.ListOpen() );
         return sb.ToString();

      }

      private string DumpOffenceHtml()
      {
         var sb = new StringBuilder();
         sb.Append( HtmlLib.ListOpen() );
         foreach ( var p in PlayerList )
         {
            if ( p.LineupPos.Trim().Length > 0 )
               if ( p.IsOffence() )
                  sb.Append( HtmlLib.ListItem( $"{p.LineupPos} {p.PlayerName}" ));
         }
         sb.Append( HtmlLib.ListOpen() );
         return sb.ToString();
      }

      public void DumpOffence()
		{
			Utility.Announce(string.Format("--{0}--Offence-------------------", TeamCode));
			foreach (var p in PlayerList)
			{
				if (p.LineupPos.Trim().Length > 0)
					if (p.IsOffence())
						AnnouncePlayer(p);
			}
		}

		private void AnnouncePlayer(NFLPlayer p)
		{
			Utility.Announce(string.Format("  {0,-5} {1,-15}",
			                               p.LineupPos, p.PlayerNameShort));
		}

		public void DumpDefence()
		{
			Utility.Announce(string.Format("--{0}--Defence-------------------", TeamCode));
			foreach (var p in PlayerList)
			{
				if (p.LineupPos.Trim().Length > 0)
					if (p.IsDefence())
						AnnouncePlayer(p);
			}
		}

		public void DumpKeyPlayers()
		{
			Utility.Announce(string.Format("{0,3} {1}", "QB", KeyPlayer("QB")));
			Utility.Announce(string.Format("{0,3} {1}", "RB", KeyPlayer("RB")));
			Utility.Announce(string.Format("{0,3} {1}", "C", KeyPlayer("C")));
			Utility.Announce(string.Format("{0,3} {1}", "DE", KeyPlayer("DE")));
			Utility.Announce(string.Format("{0,3} {1}", "MLB", KeyPlayer("MLB")));
			Utility.Announce(string.Format("{0,3} {1}", "FS", KeyPlayer("FS")));
		}

		public string KeyPlayer(string pos)
		{
			var star = "";
			var player = GetPlayerAt(pos);
			if (player != null)
				star = player.PlayerNameShort;
			else
				MissingKeys++;
			return star;
		}

		public NFLPlayer GetPlayerAt(string lineupPos)
		{
			return PlayerList.FirstOrDefault( p => IsPos( lineupPos, p.LineupPos ) );
		}

		private static bool IsPos(string posType, string actPos)
		{
			if (actPos.Trim().Length == 0) return false;

			string allPositions;
			switch (posType)
			{
				case "RB":
					allPositions = "RB,HB,TB,";
					break;
				case "MLB":
					allPositions = "MIKE,MLB,ILB,";
					break;
				case "DE":
					allPositions = "RDT,DRT,RE,RDE,DRE,RUSH,";
					break;
				case "QB":
					allPositions = "QB,";
					break;
				case "C":
					allPositions = "C,C/G,";
					break;
				case "FS":
					allPositions = "FS,";
					break;
				default:
					allPositions = "";
					break;
			}
			var isPos = !( allPositions.IndexOf(actPos + ",") < 0 );
			return isPos;
		}

		public List<NFLPlayer> LoadPlayerList( DataSet ds )
		{
			PlayerList = new List<NFLPlayer>();
			var dt = ds.Tables["lineup"];
			foreach (DataRow dr in dt.Rows)
			{
				var p = Masters.Pm.GetPlayer(dr["PLAYERID"].ToString());
				p.LineupPos = dr["POS"].ToString().Trim();
				PlayerList.Add(p);
			}
			return PlayerList;
		}
	}
}