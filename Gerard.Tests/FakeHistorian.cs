using Butler.Interfaces;
using RosterLib;
using System;

namespace Gerard.Tests
{
    public class FakeHistorian : IHistorian
    {
        public DateTime LastRun( RosterGridReport report )
        {
            return new DateTime(2014, 05, 22);
        }

        public DateTime LastRun(string reportName)
        {
            return new DateTime(2014, 05, 22);
        }
    }
}
