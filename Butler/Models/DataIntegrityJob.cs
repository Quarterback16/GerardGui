using NLog;
using RosterLib;
using RosterLib.Interfaces;
using System;

namespace Butler.Models
{
    public class DataIntegrityJob : Job
    {
        public DataIntegrityJob(IKeepTheTime timekeeper)
        {
            Name = "Data Integrity";
            //Report = new DataIntegrityReport(timekeeper);
            TimeKeeper = timekeeper;
            Logger = LogManager.GetCurrentClassLogger();
            IsNflRelated = true;
        }

        public override string DoJob()
        {
            var checker = new DataIntegrityChecker(
                season: TimeKeeper.CurrentSeason(),
                week: TimeKeeper.CurrentWeek(DateTime.Now));
            var finishedMessage = $@"Data Integrity checks completed{
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
            if (!string.IsNullOrEmpty(whyNot))
                Logger.Info("Skipped {1}: {0}", whyNot, Name);
            return (string.IsNullOrEmpty(whyNot));
        }
    }
}
