using RosterLib;
using System;
using RosterLib.Interfaces;

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
            Player = p;
            Console.WriteLine($"Prop Bet for {p.PlayerName}");
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
                Console.WriteLine($"{performance}  {propResult}");
            }
            return record;
        }
	}
}
