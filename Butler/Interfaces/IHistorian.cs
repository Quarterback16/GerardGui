using RosterLib;
using System;

namespace Butler.Interfaces
{
    public interface IHistorian
    {
        DateTime LastRun( RosterGridReport report );
        DateTime LastRun( string report);
    }
}
