using Butler.Interfaces;
using RosterLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerard.Tests
{
    public class FakeHistorian : IHistorian
    {
        public DateTime LastRun( RosterGridReport report )
        {
            return new DateTime(2014, 05, 22);
        }
    }
}
