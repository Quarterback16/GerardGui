using RosterLib;
using RosterLib.Helpers;
using RosterLib.Interfaces;
using System;

namespace Butler.Models
{
    public class GameProjectionsJob : Job
    {
        public RosterGridReport Report { get; set; }

        public ISeasonScheduler SeasonScheduler { get; set; }

        public GameProjectionsJob(IKeepTheTime timeKeeper)
        {
            Name = "Game Projections";
            var week = $"{timeKeeper.CurrentWeek(DateTime.Now):00}";
            Report = new SeasonProjectionReport(timeKeeper);
            TimeKeeper = timeKeeper;
            Logger = NLog.LogManager.GetCurrentClassLogger();
            IsNflRelated = true;
            SeasonScheduler = new SeasonScheduler();
        }

        public override string DoJob()
        {
            try
            {
                var preJob = new RankingsJob(TimeKeeper, force: true);
                var outcome = preJob.Execute();
                Logger.Info("Rankings {0}", outcome);
                var result = Report.DoReport();
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        ///   Win Projections are done once in Preseason and then once every week during the season
        /// </summary>
        /// <param name="whyNot"></param>
        /// <returns></returns>
        public override bool IsTimeTodo(out string whyNot)
        {
            base.IsTimeTodo(out whyNot);

            if (string.IsNullOrEmpty(whyNot))
            {
                if (!SeasonScheduler.ScheduleAvailable(TimeKeeper.CurrentSeason()))
                {
                    whyNot = "The schedule is not yet available for " + TimeKeeper.CurrentSeason();
                }
            }
            if (string.IsNullOrEmpty(whyNot))
            {
                //  check if there is any new data
                whyNot = Report.CheckLastRunDate();
            }
            if (string.IsNullOrEmpty(whyNot))
            {
                if (TimeKeeper.IsItPeakTime())
                    whyNot = $"{DateTime.Now.TimeOfDay:t} is peak time";
            }
            if (!string.IsNullOrEmpty(whyNot))
                Logger.Info("Skipped {1}: {0}", whyNot, Name);
            return (string.IsNullOrEmpty(whyNot));
        }
    }
}