using NLog;
using RosterLib;
using RosterLib.Interfaces;
using RosterLib.RosterGridReports;

namespace Butler.Models
{
    public class FantasyScoreCardJob : Job
    {
        public RosterGridReport Report { get; set; }

        public FantasyScoreCardJob(
            IKeepTheTime timekeeper)
        {
            Name = "Fantasy Scorecards";
            Report = new FantasyScorecardReport(
                timekeeper,
                new DbfPlayerGameMetricsDao(),
                new PlayerLister());
            TimeKeeper = timekeeper;
            Logger = LogManager.GetCurrentClassLogger();
            IsNflRelated = true;
        }

        public override string DoJob()
        {
            //  loop through all positions
            Report.RenderAsHtml(); //  the old method that does the work
            Report.Finish();
            var finishedMessage = $"Rendered {Report.Name} to {Report.OutputFilename()}";
            return finishedMessage;
        }

        public override bool IsTimeTodo(
            out string whyNot)
        {
            base.IsTimeTodo(out whyNot);

            if (string.IsNullOrEmpty(whyNot))
            {
                if (string.IsNullOrEmpty(whyNot))
                {
                    if (TimeKeeper.IsItPeakTime())
                        whyNot = "Peak time - no noise please";
                }
            }

            if (!string.IsNullOrEmpty(whyNot))
                Logger.Info("Skipped {1}: {0}", whyNot, Name);

            return (string.IsNullOrEmpty(whyNot));
        }
    }
}
