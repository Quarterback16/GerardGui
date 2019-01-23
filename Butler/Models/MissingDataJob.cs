using Butler.Interfaces;
using Butler.Messaging;
using NLog;
using RosterLib;
using RosterLib.Interfaces;
using System;

namespace Butler.Models
{
    public class MissingDataJob : Job
    {
        public IHistorian Historian { get; set; }

        public MissingDataJob(
          IKeepTheTime timekeeper,
          IHistorian historian)
        {
            Name = "Missing Data";
            TimeKeeper = timekeeper;
            Logger = LogManager.GetCurrentClassLogger();
            IsNflRelated = true;
            Historian = historian;
        }

        public override string DoJob()
        {
            var checker = new MissingPlayerDataChecker(
                new ShuttleSender(Logger));
            //TODO: expand to other teams
            checker.CheckPlayers("SF");
            var finishedMessage = $@"Missing Player Data checks completed{
                DateTime.Now
                }";
            return finishedMessage;
        }

        public override bool IsTimeTodo(out string whyNot)
        {
            whyNot = string.Empty;
            base.IsTimeTodo(out whyNot);

            if (string.IsNullOrEmpty(whyNot))
            {
                if (TimeKeeper.IsItPeakTime())
                    whyNot = "Peak time - no noise please";
            }

            if (string.IsNullOrEmpty(whyNot))
            {
                var regularity = 7;
                if (TimeKeeper.IsItPreseason())
                    regularity += 14;
                var sevenDaysAgo = DateTime.Now.Subtract(
                    new TimeSpan(regularity, 0, 0, 0)).Date;
                var lastRun = Historian.LastRun(
                    "MissingPlayerDataChecker").Date;
                if (lastRun > sevenDaysAgo)
                    whyNot = $"Has been done less than {regularity} days ago";
            }

            if (!string.IsNullOrEmpty(whyNot))
                Logger.Info("Skipped {1}: {0}", whyNot, Name);
            return (string.IsNullOrEmpty(whyNot));
        }
    }
}
