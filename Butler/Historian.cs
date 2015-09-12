using Butler.Interfaces;
using RosterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
