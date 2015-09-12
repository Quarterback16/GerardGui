using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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