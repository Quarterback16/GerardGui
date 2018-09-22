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
                "ACT",
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
            AddTeamRow(
                game: game,
                team: game.AwayNflTeam,
                projectedScore: game.BookieTip.AwayScore,
                actualScore: game.AwayScore);
            AddTeamRow(
                game: game,
                team: game.HomeNflTeam,
                projectedScore: game.BookieTip.HomeScore,
                actualScore: game.HomeScore,
                showWhen: false);
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

        private void AddTeamRow(
            NFLGame game,
            NflTeam team,
            int projectedScore,
            int actualScore,
            bool showWhen = true)
        {
            var dr = InternalTable.Body.NewRow();
            if (showWhen)
                dr["WHEN"] = TimeBit(game);
            dr["MU"] = team.TeamCode;
            dr["PRO"] = ProjectedTeamScore(projectedScore);
            if (game.Played() )
                dr["ACT"] = ActualTeamScore(actualScore);
            dr["QB"] = PickupChart.PlayerWikiPiece(
                team.PassUnit.Q1,
                game,
                YahooCalculator);
            var wikiTeam = new WikiTeam(
                team,
                game);
            dr["QBA"] = PickupChart.ActualOutput(
                game: game,
                player: team.PassUnit.Q1,
                runners: null,
                isReport:false);
            SetMainTeamRoles(dr, wikiTeam);
            InternalTable.Body.Rows.Add(dr);
        }

        private static string ProjectedTeamScore(int score)
        {
            return TeamScore(score,24);
        }

        private static string ActualTeamScore(int score)
        {
            return TeamScore(score, 34);
        }

        private static string TeamScore(int score, int limit)
        {
            if (score > limit)
                return $"**{score}**";

            return $"{score}";
        }

        private void SetMainTeamRoles(DataRow dr, WikiTeam wikiTeam)
        {
            dr["RB"] = PickupChart.GetRunnerWikiBit(
                team: wikiTeam,
                calculator: YahooCalculator);
            dr["RBA"] = PickupChart.ActualOutput(
                game: wikiTeam.Game,
                player: wikiTeam.Team.RunUnit.AceBack,
                runners: wikiTeam.Team.RunUnit.Starters,
                isReport: false);
            dr["W1"] = PickupChart.GetPlayerWikiBit(
                wikiTeam.Team.PassUnit.W1,
                team: wikiTeam,
                calculator: YahooCalculator,
                bLinks: false);
            dr["W1A"] = PickupChart.ActualOutput(
                game: wikiTeam.Game,
                player: wikiTeam.Team.PassUnit.W1,
                runners: null,
                isReport: false);
            dr["W2"] = PickupChart.GetPlayerWikiBit(
                wikiTeam.Team.PassUnit.W2,
                team: wikiTeam,
                calculator: YahooCalculator,
                bLinks: false);
            dr["W2A"] = PickupChart.ActualOutput(
                game: wikiTeam.Game,
                player: wikiTeam.Team.PassUnit.W2,
                runners: null,
                isReport: false);
            dr["TE"] = PickupChart.GetPlayerWikiBit(
                wikiTeam.Team.PassUnit.TE,
                team: wikiTeam,
                calculator: YahooCalculator,
                bLinks: false);
            dr["TEA"] = PickupChart.ActualOutput(
                game: wikiTeam.Game,
                player: wikiTeam.Team.PassUnit.TE,
                runners: null,
                isReport: false);
            dr["PK"] = PickupChart.GetPlayerWikiBit(
                wikiTeam.Team.KickUnit.PlaceKicker,
                team: wikiTeam,
                calculator: YahooCalculator,
                bLinks: false);
            dr["PKA"] = PickupChart.ActualOutput(
                game: wikiTeam.Game,
                player: wikiTeam.Team.KickUnit.PlaceKicker,
                runners: null,
                isReport: false);
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
                if (IsActualColumn(column))
                    header.Append("||");
                else
                {
                    if (IsPosition(column))
                    {
                        header.Append($"|| **{column}** ");
                    }
                    else
                        header.Append($" **{column}** ||");
                }
            }
            return header.ToString();
        }

        private static bool IsPosition(string column)
        {
            if (column == "QB"
                || column == "RB"
                || column == "W1"
                || column == "W2"
                || column == "TE"
                || column == "PK")
                return true;
            return false;
        }

        private static bool IsActualColumn(string column)
        {
            var cLastCharacter = column[column.Length - 1];
            return (cLastCharacter == 'A');
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
