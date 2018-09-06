using RosterLib;
using RosterLib.RosterGridReports;
using System;
using System.Collections.Generic;
using System.Data;

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
                "QB"
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
                dr[column] = "   ";
            }
            InternalTable.Body.Rows.Add(dr);
        }

        private void AddAwayRow(NFLGame game)
        {
            var dr = InternalTable.Body.NewRow();
            dr["WHEN"] = TimeBit(game);
            dr["MU"] = game.AwayNflTeam.TeamCode;
            dr["PRO"] = game.BookieTip.AwayScore;
            dr["QB"] = PickupChart.PlayerPiece(
                game.AwayNflTeam.PassUnit.Q1,
                game,
                YahooCalculator,
                bLinks: false);
            InternalTable.Body.Rows.Add(dr);
        }

        private void AddHomeRow(NFLGame game)
        {
            var dr = InternalTable.Body.NewRow();
            dr["WHEN"] = "    ";
            dr["MU"] = game.HomeNflTeam.TeamCode;
            dr["PRO"] = game.BookieTip.HomeScore;
            dr["QB"] = PickupChart.PlayerPiece(
                game.HomeNflTeam.PassUnit.Q1,
                game,
                YahooCalculator,
                bLinks: false);
            InternalTable.Body.Rows.Add(dr);
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
                Rows.Add(DataRowToWiki(InternalTable.Body.Rows[i]));
            }
        }

        private static string WikiTableHeader()
        {
            return "|| **When** || **MU** || **PRO** ||  **QB**  ||";
        }

        private static string DataRowToWiki(DataRow dr)
        {
            var when = dr["WHEN"].ToString();
            var matchUp = dr["MU"].ToString();
            var pro = dr["PRO"].ToString();
            var qb = dr["QB"].ToString();
            return $"||  {when}  ||  {matchUp}   ||  {pro}   || {qb}  ||";
        }
    }
}
