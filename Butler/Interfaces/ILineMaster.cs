using Butler.Implementations;
using System;

namespace Butler.Interfaces
{
    public interface ILineMaster
    {
        GameLine GetLine(
            DateTime gameDate,
            string homeTeamCode);
    }
}
