using Butler.Interfaces;
using RosterLib;
using System;

namespace Butler
{
   public class Historian : IHistorian
    {
        public DateTime LastRun(RosterGridReport report )
        {
            var lastRun = new DateTime(1, 1, 1);
            lastRun = Utility.TflWs.GetLastRun(report.Name);
            return lastRun;
        }

        public DateTime LastRun(string reportName)
        {
            var lastRun = new DateTime(1, 1, 1);
            lastRun = Utility.TflWs.GetLastRun(reportName);
            return lastRun;
        }
    }
}
