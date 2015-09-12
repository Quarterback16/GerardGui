using System;
using System.Data;
using System.Linq;


namespace RosterLib
{
   /// <summary>
   ///   PlayerProjection outputs a sheet of season projections on a player.
   /// </summary>
   public class PlayerProjection
   {
      public NFLPlayer Player { get; set; }

      public string Season { get; set; }

      public PlayerProjection(string playerId, string season)
      {
         Player = new NFLPlayer(playerId);
         Season = season;
         Player.LoadSeason(Season);
      }

      public PlayerProjection(NFLPlayer player, string season)
      {
         Player = player;
         Season = season;
         Player.LoadSeason(Season);
      }

      /// <summary>
      ///   Creates the output.
      /// </summary>
      public void Render()
      {
         var str = new SimpleTableReport("Player Projection " + Player.PlayerName + "-" + Season);
         str.AddStyle(
            "#container { text-align: left; background-color: #ccc; margin: 0 auto; border: 1px solid #545454; width: 641px; padding:10px; font: 13px/19px Trebuchet MS, Georgia, Times New Roman, serif; }");
         str.AddStyle("#main { margin-left:1em; }");
         str.AddStyle("#dtStamp { font-size:0.8em; }");
         str.AddStyle(".end { clear: both; }");
         str.AddStyle(".gponame { color:white; background:black }");
         str.AddStyle(
            "label { display:block; float:left; width:130px; padding: 3px 5px; margin: 0px 0px 5px 0px; text-align:right; }");
         str.AddStyle(
            "value { display:block; float:left; width:100px; padding: 3px 5px; margin: 0px 0px 5px 0px; text-align:left; font-weight: bold; color:blue }");
         str.AddStyle(
            "#notes { float:right; height:auto; width:308px; font-size: 88%; background-color: #ffffe1; border: 1px solid #666666; padding: 5px; margin: 0px 0px 10px 10px; color:#666666 }");
         str.AddStyle(
            "div.notes H4 { background-image: url(images/icon_info.gif); background-repeat: no-repeat; background-position: top left; padding: 3px 0px 3px 27px; border-width: 0px 0px 1px 0px; border-style: solid; border-color: #666666; color: #666666; font-size: 110%;}");
         str.ColumnHeadings = true;
         str.DoRowNumbers = false;
         str.ShowElapsedTime = false;
         str.IsFooter = false;
         str.AddColumn(new ReportColumn("Week", "WEEK", "{0}"));
         str.AddColumn(new ReportColumn("Matchup", "MATCH", "{0}"));
         str.AddColumn(new ReportColumn("Score", "SCORE", "{0}"));
         str.AddColumn(new ReportColumn("OppUnit", "OPPRATE", "{0}"));
         str.AddColumn(new ReportColumn("Projection", "PROJ", "{0:0.0}", true));
         str.AddColumn(new ReportColumn("Y-FP", "FP", "{0:0.0}", true));
         str.AddColumn(new ReportColumn("Stats", "STATS", "{0}"));
         str.AddColumn(new ReportColumn("Actual", "ACTUAL", "{0:0.0}", true));
         str.AddColumn(new ReportColumn("Variation", "VAR", "{0:0.0}"));
         str.LoadBody(BuildTable());
         str.SubHeader = SubHeading();
         str.RenderAsHtml(FileName(), true);
      }

      public string FileName()
      {
         return string.Format("{0}{2}//Projections//{1}.htm", Utility.OutputDirectory(),
                              Player.PlayerCode, Season);
      }

      private DataTable BuildTable()
      {
         var c = new YahooCalculator();
         var dt = new DataTable();
         var cols = dt.Columns;
         cols.Add("WEEK", typeof (String));
         cols.Add("MATCH", typeof (String));
         cols.Add("SCORE", typeof (String));
         cols.Add("OPPRATE", typeof (String));
         cols.Add("PROJ", typeof (Int16));
         cols.Add("FP", typeof (Decimal));
         cols.Add("STATS", typeof (String));
         cols.Add("ACTUAL", typeof (Int16));
         cols.Add("VAR", typeof (Int16));

         var gameCount = 1;
         var byeDone = false;
         foreach (var g in Player.CurrentGames)
         {
            var dr = dt.NewRow();
            g.LoadPrediction( "unit" );
            if ((g.WeekNo > gameCount) && (! byeDone))
            {
               dr["WEEK"] = gameCount;
               dr["MATCH"] = "BYE";
               dr["SCORE"] = "";
               dr["OPPRATE"] = "";
               dr["PROJ"] = 0;
               dr["FP"] = 0;
               dr["STATS"] = "";
               dr["ACTUAL"] = 0;
               dr["VAR"] = 0;
               dt.Rows.Add(dr);
               dr = dt.NewRow();
               byeDone = true;
            }
            dr["WEEK"] = g.WeekNo;
            dr["MATCH"] = g.OpponentOut(Player.CurrTeam.TeamCode);
            dr["SCORE"] = g.ProjectedScoreOut(Player.CurrTeam.TeamCode);
            dr["OPPRATE"] = OppUnitRating(g, Player.CurrTeam.TeamCode, Player.PlayerCat);
            dr["PROJ"] = GameProjection(g, Player.CurrTeam.TeamCode, Player.PlayerCat, Player.PlayerRole);
            dr["STATS"] = Player.StatsFor(g, Player.CurrTeam.TeamCode);
            dr[ "FP" ] = c.Calculate( Player, g ).Player.Points ;
            dr["ACTUAL"] = ActualScores(g);
            dr["VAR"] = 0;
            dt.Rows.Add(dr);
            gameCount++;
         }
         return dt;
      }

      private int ActualScores(NFLGame g)
      {
         var nScores = 0;
         DataSet ds = null;

         switch (Player.PlayerCat)
         {
            case "1":
               ds = Player.LastScores(Constants.K_SCORE_TD_PASS, g.WeekNo, g.WeekNo, g.Season, "2");
               break;
            case "2":
               ds = Player.LastScores(Constants.K_SCORE_TD_RUN, g.WeekNo, g.WeekNo, g.Season, "1");
               break;
            case "3":
               ds = Player.LastScores(Constants.K_SCORE_TD_CATCH, g.WeekNo, g.WeekNo, g.Season, "1");
               break;
            case "4":
               ds = Player.LastScores(Constants.K_SCORE_FIELD_GOAL, g.WeekNo, g.WeekNo, g.Season, "1");
               break;
         }
         if (ds != null)
            nScores += ds.Tables[0].Rows.Cast<DataRow>().Count();
         return nScores;
      }

      public static int GameProjection(NFLGame g, string playerTeamCode, string playerCategory, string playerRole)
      {
         var projection = 0;
         if (playerRole == Constants.K_ROLE_STARTER)
         {
            switch (playerCategory)
            {
               case Constants.K_QUARTERBACK_CAT:
                  projection = g.IsHome(playerTeamCode) ? g.ProjectedHomeTdp : g.ProjectedAwayTdp;
                  break;
               case Constants.K_RUNNINGBACK_CAT:
                  projection = g.IsHome(playerTeamCode) ? g.ProjectedHomeTdr : g.ProjectedAwayTdr;
                  break;
               case Constants.K_RECEIVER_CAT:
                  projection = g.IsHome(playerTeamCode) ? g.ProjectedHomeTdp/2 : g.ProjectedAwayTdp/2;
                  break;
               case Constants.K_KICKER_CAT:
                  projection = g.IsHome( playerTeamCode ) ? g.ProjectedHomeFg : g.ProjectedAwayFg;
                  break;
            }
#if DEBUG
            Utility.Announce(string.Format("Game {0} projection for {1} {2} {3} is {4}",
               g.GameKey(), playerTeamCode, playerCategory, playerRole, projection ) );
#endif
         }
         return projection;
      }

      private static string OppUnitRating(NFLGame g, string playerTeamCode, string playerCategory)
      {
         var unitRate = "X";
         var rating = g.OpponentTeam(playerTeamCode).Ratings;
         switch (playerCategory)
         {
            case "1":
               unitRate = rating.Substring(5, 1);
               break;
            case "2":
               unitRate = rating.Substring(4, 1);
               break;
            case "3":
               unitRate = rating.Substring(5, 1);
               break;
         }
         //return string.Format("{0}-{1}-{2}", rating, playerCategory, unitRate );
         return unitRate;
      }

      private string SubHeading()
      {
         var header = Legend();
         var div = HtmlLib.DivOpen("id=\"main\"") + PlayerData() + EndDiv() + HtmlLib.DivClose();
         return string.Format("{0}{1}\n", header, div);
      }

      private string Legend()
      {
         var status = Player.Status();
         var lastSeason = "";
         if (Player.IsRetired) lastSeason = Player.LastSeason;
         return string.Format("\n<h3>{0} - {1} {2}</h3>\n", Player.PlayerName, status, lastSeason);
      }

      private static string EndDiv()
      {
         return HtmlLib.DivOpen("class=\"end\"") + HtmlLib.Spaces(1) + HtmlLib.DivClose() + "\n";
      }

      private string PlayerData()
      {
         var s = String.Empty;
         Player.SetDraftRound();
         Player.CalculateEp(Season);
         s += DataOut("Position", Player.PlayerPos);
         s += DataOut("Role", Player.RoleOut());
         s += DataOut("Ratings", Player.TeamRating(Utility.PreviousSeason(Season)));
         s += DataOut("Rookie Yr", Player.RookieYear);
         s += DataOut("Acquired", Player.Drafted);
         s += DataOut("Seasons", Player.Seasons());
         s += DataOut("Scores per yr", String.Format("{0:##.#}", Player.ScoresPerYear()));
         s += DataOut("Age", Player.PlayerAge());
         s += DataOut("Scores", String.Format("{0:##.#}", Player.Scores));
         s += DataOut("Experience Pts", Player.ExperiencePoints.ToString());
         s += DataOut("Current Value", Player.Value().ToString());

         return s;
      }


      private static string DataOut(string label, string val)
      {
         return string.Format("<label>{0}:</label> <value>{1,8}</value>", label, val);
      }
   }
}