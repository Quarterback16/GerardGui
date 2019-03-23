using Butler.Interfaces;
using NLog;
using RosterLib;
using RosterLib.Interfaces;
using RosterLib.RosterGridReports;
using System;

namespace Butler.Models
{
    public class PreviousPickupChartJob : Job
    {
        public IHistorian Historian { get; set; }

        public int Week { get; set; }

        public RosterGridReport Report { get; set; }

        public PreviousPickupChartJob(
            IKeepTheTime timekeeper,
            IHistorian historian)
        {
            Name = "Previous Pickup Chart";
            TimeKeeper = timekeeper;
            Logger = LogManager.GetCurrentClassLogger();
            Historian = historian;

            Week = Int32.Parse(TimeKeeper.PreviousWeek());

            Report = new PickupChart(TimeKeeper, Week);
        }

        public override string DoJob()
        {
            Report.RenderAsHtml(); //  the method that does the work
            Report.Finish();
            return $"Rendered {Report.Name} to {Report.OutputFilename()}";
        }

        public override bool IsTimeTodo(out string whyNot)
        {
            whyNot = string.Empty;
            if (OnHold())
                whyNot = "Job is on hold";

            if (string.IsNullOrEmpty(whyNot))
            {
                if (!TimeKeeper.IsItRegularSeason())
                    whyNot = "Its not the regular season yet";
            }

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