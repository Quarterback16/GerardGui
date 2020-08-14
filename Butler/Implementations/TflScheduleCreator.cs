using Butler.Interfaces;
using Butler.Models;
using System;
using System.Data.OleDb;

namespace Butler.Implementations
{
    public class TflScheduleCreator : IGameProcessor
    {
        public OleDbConnection OleDbConn;

        public TflScheduleCreator()
        {
            //  set up minimal TFL Data Librarian functionality
            OleDbConn = new OleDbConnection(
                "Provider=VFPOLEDB.1;Data Source=e:\\tfl\\nfl\\team.dbf");
        }
        public void ProcessGame(
            Game g,
            int n)
        {
            var season = Season(g.GameDate);
            var week = TflWeek(g.Round);
            var gameNumber = GameNumber(n);
            var gameDate = g.GameDate;
            var gameHour = GameHour(g.GameDate);
            var awayTeamCode = g.AwayTeam;
            var homeTeamCode = g.HomeTeam;

            InsertGame(
                season,
                week,
                gameNumber,
                gameDate,
                gameHour,
                awayTeamCode,
                homeTeamCode);
        }

        public static string GameNumber(
            int n)
        {
            var str = char.ConvertFromUtf32(n + 64);
            return str;
        }

        public static string TflWeek(
            int round)
        {
            return $"{round:0#}";
        }

        public static string Season(
            DateTime gameDate)
        {
            var yr = gameDate.Year;
            var mth = gameDate.Month;
            var season = yr;
            if (mth < 3)
                season--;
            return season.ToString();
        }

        public static string GameHour(
            DateTime gameDate)
        {
            // Assume times are US Eastern Standard time
            // Common times are 13:00 -> "1"
            //  16:05 -> "4"
            //  16:25 -> "5"
            //  19:15
            //  22:10
            //  20:20
            //
            //  formula get hr, substract 12, if > 9 set to 9
            //  spec case make 16:25 a "5"
            //  morning games get a 0

            if (gameDate.ToString("HH:mm") == "16:25")
                return "5";

            var hr = gameDate.Hour;
            hr -= 12;
            if (hr > 9)
                hr = 9;
            if (hr < 0)
                hr = 0;
            return hr.ToString();
        }

        public string InsertGame(
            string season,
            string week,
            string gameNumber,
            DateTime gameDate,
            string gameHour,
            string awayTeamCode,
            string homeTeamCode)
        {
            var formatStr =
               "INSERT INTO SCHED (SEASON, WEEK, GAMENO, GAMEDATE, GAMEHOUR, AWAYTEAM, HOMETEAM, GAMELIVE, STADIUM, AWAYSCORE, HOMESCORE, SPREAD, TOTAL, MYTIP, HOME_TDP, AWAY_TDP, HOME_TDR, AWAY_TDR, HOME_FG, AWAY_FG, HOME_TDD, AWAY_TDD, HOME_TDS, AWAY_TDS, ID)";

            formatStr += " VALUES( '{0}','{1}','{2}',{{{3:MM/dd/yyyy}}},'{4}','{5}','{6}', .f., ' ', 0, 0, 0, 0.0, '  ',0,0,0,0,0,0,0,0,0,0,0 )";
            var commandStr = string.Format(
                formatStr,
                season,
                week,
                gameNumber,
                gameDate,
                gameHour,
                awayTeamCode,
                homeTeamCode);

            ExecuteNflCommand(
                commandStr);

            return $"{season}:{week}:{gameNumber}";
        }

        private void ExecuteNflCommand(
            string commandStr)
        {
            OleDbConn.Close();
            OleDbConn.Open();
            using (var cmd = new OleDbCommand(
                commandStr,
                OleDbConn))
            {
                cmd.ExecuteNonQuery();
                OleDbConn.Close();
            }
        }
    }
}
