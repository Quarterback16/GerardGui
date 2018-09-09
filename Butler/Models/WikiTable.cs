using RosterLib;
using RosterLib.RosterGridReports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Butler.Models
{
    public class WikiTable
    {
        public SimpleTableReport InternalTable { get; set; }
        public List<string> Rows { get; set; }
        public List<string> Cols { get; set; }
        public PickupChart PickupChart { get; set; }
        public YahooCalculator YahooCalculator { get; set; }

        public WikiTable(int week)
        {
            InternalTable = new SimpleTableReport();
            Rows = new List<string>();
            SetupColumns();
            InternalTable.Body = BuildTable();
            PickupChart = new PickupChart(
                new TimeKeeper(null),
                week );
            YahooCalculator = new YahooCalculator();
        }

        private void SetupColumns()
        {
            Cols = new List<string>
            {
                "WHEN",
                "MU",
                "PRO",
                "QB",
                "QBA",
                "RB",
                "RBA",
                "W1",
                "W1A",
                "W2",
                "W2A",
                "TE",
                "TEA",
                "PK",
                "PKA",
                "Notes"
            };
        }

        private DataTable BuildTable()
        {
            var dt = new DataTable();
            var cols = dt.Columns;
            foreach (var column in Cols)
            {
                cols.Add(column, typeof(String));
            }
            return dt;
        }

        public void AddColumn(ReportColumn column)
        {
            InternalTable.AddColumn(column);
        }

        internal void AddGame(NFLGame game)
        {
            game.CalculateSpreadResult();
            LoadUnits(game);
            AddAwayRow(game);
            AddHomeRow(game);
            AddBlankRow();
        }

        private static void LoadUnits(NFLGame game)
        {
            LoadUnits(game.HomeNflTeam);
            LoadUnits(game.AwayNflTeam);
        }

        private static void LoadUnits(NflTeam team)
        {
            team.LoadKickUnit();
            team.LoadRushUnit();
            team.LoadPassUnit();
        }

        private void AddBlankRow()
        {
            var dr = InternalTable.Body.NewRow();
            foreach (var column in Cols)
            {
                dr[column] = "  ";
            }
            InternalTable.Body.Rows.Add(dr);
        }

        private void AddAwayRow(NFLGame game)
        {
            var dr = InternalTable.Body.NewRow();
            dr["WHEN"] = TimeBit(game);
            dr["MU"] = game.AwayNflTeam.TeamCode;
            dr["PRO"] = game.BookieTip.AwayScore;
            dr["QB"] = PickupChart.PlayerWikiPiece(
                game.AwayNflTeam.PassUnit.Q1,
                game,
                YahooCalculator);
            var wikiTeam = new WikiTeam(
                game.AwayNflTeam,
                game);
            SetMainTeamRoles(dr, wikiTeam);

            InternalTable.Body.Rows.Add(dr);
        }

        private void AddHomeRow(NFLGame game)
        {
            var dr = InternalTable.Body.NewRow();
            dr["WHEN"] = "    ";
            dr["MU"] = game.HomeNflTeam.TeamCode;
            dr["PRO"] = game.BookieTip.HomeScore;
            dr["QB"] = PickupChart.PlayerWikiPiece(
                game.HomeNflTeam.PassUnit.Q1,
                game,
                YahooCalculator);
            var wikiTeam = new WikiTeam(
                game.HomeNflTeam,
                game);
            SetMainTeamRoles(dr, wikiTeam);
            InternalTable.Body.Rows.Add(dr);
        }

        private void SetMainTeamRoles(DataRow dr, WikiTeam wikiTeam)
        {
            dr["RB"] = PickupChart.GetRunnerWikiBit(
                team: wikiTeam,
                calculator: YahooCalculator);
            dr["W1"] = PickupChart.GetPlayerWikiBit(
                wikiTeam.Team.PassUnit.W1,
                team: wikiTeam,
                calculator: YahooCalculator,
                bLinks: false);
            dr["W2"] = PickupChart.GetPlayerWikiBit(
                wikiTeam.Team.PassUnit.W2,
                team: wikiTeam,
                calculator: YahooCalculator,
                bLinks: false);
            dr["TE"] = PickupChart.GetPlayerWikiBit(
                wikiTeam.Team.PassUnit.TE,
                team: wikiTeam,
                calculator: YahooCalculator,
                bLinks: false);
            dr["PK"] = PickupChart.GetPlayerWikiBit(
                wikiTeam.Team.KickUnit.PlaceKicker,
                team: wikiTeam,
                calculator: YahooCalculator,
                bLinks: false);
        }

        private static string TimeBit(NFLGame game)
        {
            var dayName = game.GameDate.ToString("dddd").Substring(0, 2);
            var bit = $"{dayName}{game.Hour}";
            return bit;
        }

        internal void GenerateRows()
        {
            Rows.Add(WikiTableHeader());
            for (int i = 0; i < InternalTable.Body.Rows.Count; i++)
            {
                Rows.Add(
                    DataRowToWiki(
                        InternalTable.Body.Rows[i]));
            }
        }

        private string WikiTableHeader()
        {
            var header = new StringBuilder();
            header.Append("||");
            foreach (var column in Cols)
            {
                header.Append(" **");
                header.Append(column);
                header.Append("** ||");
            }
            return header.ToString();
        }

        private string DataRowToWiki(DataRow dr)
        {
            var line = new StringBuilder();
            line.Append("||");
            foreach (var column in Cols)
            {
                var contents = dr[column].ToString();
                line.Append(" ");
                line.Append(contents);
                line.Append(" ||");
            }
            return line.ToString();
        }

        private class WikiTeam : IWinOrLose
        {
            public WikiTeam(NflTeam team, NFLGame game)
            {
                Game = game;
                Team = team;
            }

            public NFLGame Game { get; set; }
            public NflTeam Team { get; set; }
            public bool Home { get; set; }
            public bool IsWinner { get; set; }
            public decimal Margin { get; set; }
        }
    }
}
