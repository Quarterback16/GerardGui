using Butler.Implementations;
using Butler.Interfaces;
using NLog;
using RosterLib;
using RosterLib.Interfaces;
using System;

namespace Butler.Models
{
    public class LoadLineJob : Job
    {
        public ILineMaster LineMaster { get; set; }

        public LoadLineJob(
            IKeepTheTime timekeeper,
            ILineMaster lineMaster)
        {
            Name = "Load Line";
            TimeKeeper = timekeeper;
            Logger = LogManager.GetCurrentClassLogger();
            IsNflRelated = true;
            LineMaster = lineMaster;
        }

        public override bool IsTimeTodo(
            out string whyNot)
        {
            base.IsTimeTodo(
                out whyNot);

            if (string.IsNullOrEmpty(whyNot))
            {
                if (TimeKeeper.IsItPeakTime())
                    whyNot = "Peak time - no noise please";
            }

            if (!string.IsNullOrEmpty(whyNot))
                Logger.Info("Skipped {1}: {0}", whyNot, Name);

            return string.IsNullOrEmpty(whyNot);
        }

        public override string DoJob()
        {
            if (LineMaster == null)
                return "No Line Master available";

            var checkCount = 0;
            var currWeek = TimeKeeper.CurrentWeek(
                DateTime.Now);
            var week = new NFLWeek(
                seasonIn: 2020,
                weekIn: currWeek,
                loadGames: true);
            var gameList = week.GameList();
            foreach (NFLGame game in gameList)
            {
                var gameLine = LineMaster.GetLine(
                    game.GameDate,
                    game.HomeTeam);
                if ( ! gameLine.IsEmpty() )
                {
                    if (game.Spread != gameLine.Spread
                        || game.Total != gameLine.Total)
                    {
                        UpdateGameLine(
                            game,
                            gameLine);
                    }
                }
                checkCount++;
            }

            var finishedMessage = $@"{checkCount} Game Lines checked at {
                DateTime.Now
                }";
            Logger.Info(finishedMessage);
            return finishedMessage;
        }

        private static void UpdateGameLine(
            NFLGame game,
            GameLine gameLine)
        {
            game.StoreGameLine(
                gameLine.Spread,
                gameLine.Total);

            Console.WriteLine(
                $"Updating {game} : {gameLine}");
        }
    }
}
