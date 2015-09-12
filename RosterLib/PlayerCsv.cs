﻿using System;
using System.Collections.Generic;

namespace RosterLib
{
   public class PlayerCsv : RosterGridReport
   {
      public string LeagueCode { get; set; }

      public PlayerLister Lister { get; set; }

      public List<StarterConfig> Configs { get; set; }

      public bool DoProjections { get; set; }

      public PlayerCsv()
      {
         Name = "Players CSV";
         SetLastRunDate();
         Lister = new PlayerLister();
         Configs = new List<StarterConfig>
         {
            new StarterConfig {Category = Constants.K_QUARTERBACK_CAT, Position = "QB"},
            new StarterConfig {Category = Constants.K_RUNNINGBACK_CAT, Position = "RB"},
            new StarterConfig {Category = Constants.K_RECEIVER_CAT, Position = "WR"},
            new StarterConfig {Category = Constants.K_RECEIVER_CAT, Position = "TE"},
            new StarterConfig {Category = Constants.K_KICKER_CAT, Position = "K"}
         };
      }

      public override void RenderAsHtml()
      {
         RenderPlayerCsv();
      }


      public string RenderPlayerCsv()
      {
         Lister.SortOrder = "CURSCORES DESC";

         var nWeek = Int32.Parse(Utility.CurrentWeek());
         if (nWeek == 0) nWeek = 1;

         var theWeek = new NFLWeek(Int32.Parse(Utility.CurrentSeason()), nWeek, loadGames:false);
         var scorer = new YahooScorer(theWeek);

         var weekMaster = new WeekMaster();

         Lister.RenderToCsv = true;
         Lister.SetScorer(scorer);
         Lister.StartersOnly = true;

         foreach (var sc in Configs)
         {
            Lister.Collect(sc.Category, sc.Position, string.Empty);
         }

         Lister.Folder = "Starters";
         Lister.LongStats = true;
         Lister.RenderToHtml = false;

         var fileOut = DoProjections ? Lister.RenderProjection("PlayerCsv", weekMaster) : Lister.Render("PlayerCsv");

         Lister.Clear();

         return fileOut;
      }

   }

}