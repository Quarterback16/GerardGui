using Butler.Implementations;
using NLog;
using RosterLib.Interfaces;
using System;

namespace Butler.Models
{
    public class LoadLineJob : Job
    {
        public LoadLineJob(
            IKeepTheTime timekeeper)
        {
            Name = "Load Line";
            TimeKeeper = timekeeper;
            Logger = LogManager.GetCurrentClassLogger();
            IsNflRelated = true;
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
            var checkCount = 0;

            var lineMaster = new LineMaster(
                );

            var startDate = TimeKeeper.GetDate();

            // get the upcoming games
            // foreach game see if there are any lines
            //   compare current line with new line
            //   update if changed

            var finishedMessage = $@"{checkCount} Game Lines Checked{
                DateTime.Now
                }";
            return finishedMessage;
        }
    }
}
