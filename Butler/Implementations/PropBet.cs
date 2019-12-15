using RosterLib;
using System;

namespace Butler.Implementations
{
	public class PropBet
	{
        public NFLPlayer Player { get; set; }
        public string StatType { get; set; }
        public int Quantity { get; set; }

        public PropBet()
        {
        }

        public Record Calculate(
            NFLPlayer p,
            string statType,
            int quantity)
        {
            return Calculate(
                p,
                statType,
                quantity,
                GameScenario.None);
        }

        public Record Calculate(
            NFLPlayer p,
            string statType,
            int quantity,
            GameScenario gameScenario = GameScenario.ShortFavourite )
        {
            Player = p;
            Console.WriteLine($@"Prop Bet for {
                p.PlayerName
                } {
                PropScenario(gameScenario)
                }");
            Console.WriteLine();
            StatType = statType;
            Quantity = quantity;
            var record = new Record();
            // get performances
            Player.LoadPerformances(
                allGames: false,
                currSeasonOnly: true,
                whichSeason: "2019");
            foreach (NflPerformance performance in Player.PerformanceList)
            {
                if (performance.Game == null)
                    continue;
                if (! performance.Game.Played())
                    continue;
                if (!performance.PlayerPlayed())
                    continue;
                if (   !(gameScenario == GameScenario.None)
                    && !performance.GameMatchesScenario(gameScenario))
                    continue;

                string propResult;
                if (performance.PerfStats.YDp >= Quantity
                    && performance.Game.WinningTeamCode() == Player.TeamCode)
                {
                    record.Wins++;
                    propResult = "Win";
                }
                else
                {
                    record.Losses++;
                    propResult = "Loss";
                }
                var result = $"{performance}  {propResult}";
                result = result.Replace("&nbsp;", " ");
                Console.WriteLine(result);
            }
            return record;
        }

        private static string PropScenario(
            GameScenario gameScenario)
        {
            if (gameScenario == GameScenario.None)
                return String.Empty;

            return gameScenario.ToString();
        }
    }

}
