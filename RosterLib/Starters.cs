using System;
using System.Collections.Generic;

namespace RosterLib
{
   public class Starters : RosterGridReport
   {
      public bool PlayoffsOnly { get; set; }

      public bool DoCsv { get; set; }

      public bool WriteProjectionReports { get; set; }

      public string LeagueCode { get; set; }

      public PlayerLister Lister { get; set; }

      public List<StarterConfig> Configs { get; set; }

      public List<RosterGridLeague> Leagues { get; set; }

      public Starters( bool doCsv )
      {
         Name = "Starters";
         SetLastRunDate();
         DoCsv = doCsv;
         Lister = new PlayerLister();
         Configs = new List<StarterConfig>();
         Configs.Add(new StarterConfig { Category = Constants.K_QUARTERBACK_CAT, Position = "QB" });
#if ! DEBUG2
         Configs.Add(new StarterConfig { Category = Constants.K_RUNNINGBACK_CAT, Position = "RB" });
         Configs.Add(new StarterConfig { Category = Constants.K_RECEIVER_CAT, Position = "WR" });
         Configs.Add(new StarterConfig { Category = Constants.K_RECEIVER_CAT, Position = "TE" });
         Configs.Add(new StarterConfig { Category = Constants.K_KICKER_CAT, Position = "K" });
#endif
         Leagues = new List<RosterGridLeague> {new RosterGridLeague {Id = Constants.K_LEAGUE_Yahoo, Name = "Yahoo"}};
      }


      public override void RenderAsHtml()
      {
         foreach (var league in Leagues)
         {
            LeagueCode = league.Id;
            foreach ( var rpt in Configs )
               GenerateReport( rpt );
         }
      }

      private void GenerateReport(StarterConfig rpt)
      {
         WriteProjectionReports = true;  //  Will gnerate a page for each player 
         RenderStarters(rpt.Category, rpt.Position, LeagueCode);
      }

      public string RenderStarters(string cat, string sPos, [System.Runtime.InteropServices.Optional] string fantasyLeague)
      {
         Lister.SortOrder = Int32.Parse(Utility.CurrentWeek()) > 0 ? "POINTS DESC" : "CURSCORES DESC";
         PlayoffsOnly = PlayoffsOnly;

         var theWeek = new NFLWeek(Int32.Parse(Utility.CurrentSeason()), Int32.Parse(Utility.CurrentWeek()), false);
         var gs = new GS4Scorer(theWeek);
         Lister.RenderToCsv = DoCsv;
         Lister.SetScorer(gs);
         Lister.StartersOnly = true;
         Lister.Clear();
         Lister.Collect(cat, sPos, fantasyLeague);
         Lister.Folder = "Starters";

         var fileOut = Lister.Render(string.Format("{1}-Starters-{0}", sPos, fantasyLeague));

         if (WriteProjectionReports)
            WritePlayerProjectionReports();

         Lister.Clear();

         return fileOut;
      }

      public void WritePlayerProjectionReports()
      {
         foreach (NFLPlayer p in Lister.PlayerList)
            p.PlayerProjection(Utility.CurrentSeason());
      }
   }

   public class StarterConfig
   {
      public string Category { get; set; }

      public string Position { get; set; }
   }
}