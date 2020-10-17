using Butler.Dto;
using Butler.Interfaces;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Butler.Implementations
{
    public class LineMaster : ILineMaster
    {
        public Logger Logger { get; private set; }

        public Dictionary<string,string> TeamCode { get; set; }
        public Dictionary<string, GameLine> Lines { get; set; }
        public LineMaster()
        {
            Logger = LogManager.GetCurrentClassLogger();
            LoadTeamCodes();
            Lines = new Dictionary<string, GameLine>();
            LoadLatestLines();
        }

        private void LoadLatestLines()
        {
            // for the next 7 days
            for (int i = 0; i < 3; i++)
            {
                //   get lines from API
                var gameDate = DateTime.Now.AddDays(i);
                var lines = LoadLines(
                    gameDate);
                //   add to memory
                foreach (var item in lines)
                {
                    AddGameLine(
                        item,
                        gameDate);
                }
            }
            foreach (var item in Lines)
                Info(item.ToString());
        }


        public void AddGameLine(
            GameLineDto gameLineDto,
            DateTime gameDate)
        {
            var game = gameLineDto.Game;
            var gameKey = KeyFromGameLineDto(
                game,
                gameDate);
            var gameLine = new GameLine
            {
                Spread = Math.Abs(gameLineDto.Spread),
                Total = gameLineDto.Total
            };
            Lines.Add(
                gameKey,
                gameLine);
        }

        public string KeyFromGameLineDto(
            string game,
            DateTime gameDate)
        {
            //  will be in the format XX @ XX
            var team = game.Split('@');
            var homeTeam = team[1].Trim();
            var tflHomeTeam = TeamFor(homeTeam);
            return KeyFor(
                gameDate,
                tflHomeTeam);
        }

        private void LoadTeamCodes()
        {
            TeamCode = new Dictionary<string, string>
            {
                {  "SF", "SF"},
                {  "LA" ,"LR"},
                {  "WAS","WR" },
                {  "PHI","PE" },
                {  "PIT","PS" },
                {  "ARI","AC" },
                {  "NYJ","NJ" },
                {  "LV" ,"OR" },
                {  "KC" ,"KC" },
                {  "JAX","JJ" },
                {  "HOU","HT" },
                {  "CIN","CI" },
                {  "BAL","BR" },
                {  "CAR","CP" },
                {  "ATL","AF" },
                {  "MIA","MD" },
                {  "NYG","NG" },
                {  "DAL","DC" },
                {  "IND","IC" },
                {  "CLE","CL" },
                {  "MIN","MV" },
                {  "SEA","SS" },
                {  "DEN","DB" },
                {  "NE" ,"NE" },
                {  "BUF","BB" },
                {  "TEN","TT" },
                {  "CHI","CH" },
                {  "TB", "TB" },
                {  "NO" ,"NO" },
                {  "LAC","LC" },
                {  "GB" ,"GB" },
                {  "DET","DL" }
            };
            Info($"Loaded {TeamCode.Count} team codes");
        }

        public GameLine GetLine(
            DateTime gameDate,
            string homeTeamCode)
        {
            var result = new GameLine();
            var key = KeyFor(
                gameDate,
                homeTeamCode);
            if (Lines.ContainsKey(key))
                return Lines[key];
            return result;
        }

        private static string KeyFor(
            DateTime gameDate,
            string homeTeamCode)
        {
            return $"{gameDate:yyyy-MM-dd}:{homeTeamCode}";
        }

        public void Load()
        {
            try
            {
                const string url = "https://localhost:44344/line";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(
                    url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(
                    httpResponse.GetResponseStream()))
                {
                    var rawResponse = streamReader.ReadToEnd();
                    var dto = JsonConvert.DeserializeObject<IEnumerable<GameLineDto>>(
                        rawResponse);

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static List<GameLineDto> LoadLines(
            DateTime forDate)
        {
            try
            {
                var dto = new List<GameLineDto>();
                var url = $@"https://localhost:44344/line/{forDate.ToString("yyyyMMdd")}";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(
                    url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(
                    httpResponse.GetResponseStream()))
                {
                    var rawResponse = streamReader.ReadToEnd();
                    dto = (List<GameLineDto>) JsonConvert.DeserializeObject<IEnumerable<GameLineDto>>(
                        rawResponse);

                }
                return dto;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string TeamFor(
            string msfTeamCode)
        {
            return TeamCode[msfTeamCode];
        }

        private void Info(
            string message)
        {
            Logger.Info(
                message);
            Console.WriteLine(
                message);
        }
    }
}
