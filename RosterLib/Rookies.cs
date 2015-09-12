﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
   public class Rookies : RosterGridReport
   {
      public string LeagueCode { get; set; }

      public PlayerLister Lister { get; set; }

      public List<RookieConfig> Configs { get; set; }

      public List<RosterGridLeague> Leagues { get; set; }

      public Rookies()
      {
         Name = "Rookies";
         SetLastRunDate();
         Lister = new PlayerLister();
         Configs = new List<RookieConfig>();
         Configs.Add(new RookieConfig { Category = Constants.K_QUARTERBACK_CAT, Position = "QB" });
         Configs.Add(new RookieConfig { Category = Constants.K_RUNNINGBACK_CAT, Position = "RB" });
         Configs.Add(new RookieConfig { Category = Constants.K_RECEIVER_CAT, Position = "WR" });
         Configs.Add(new RookieConfig { Category = Constants.K_RECEIVER_CAT, Position = "TE" });
         Configs.Add(new RookieConfig { Category = Constants.K_KICKER_CAT, Position = "K" });
         Leagues = new List<RosterGridLeague>();
         Leagues.Add(new RosterGridLeague { Id = Constants.K_LEAGUE_Gridstats_NFL1, Name = "Gridstats GS1" });
      }

      public override void RenderAsHtml()
      {
         foreach (var league in Leagues)
         {
            LeagueCode = league.Id;
            foreach (RookieConfig rpt in Configs)
            {
               GenerateReport(rpt, LeagueCode);
            }
         }
      }

      private string GenerateReport(RookieConfig rpt, string fantasyLeague )
      {
         Lister.StartersOnly = false;
         Lister.Clear();
         Lister.Collect(rpt.Category, rpt.Position, fantasyLeague, Utility.CurrentSeason() );
         Lister.Folder = "Rookies";

         var fileOut = Lister.Render(string.Format("{1}-Rookies-{0}", rpt.Position, fantasyLeague));

         Lister.Clear();

         return fileOut;

      }
   }
   public class RookieConfig
   {
      public string Category { get; set; }

      public string Position { get; set; }
   }
}
