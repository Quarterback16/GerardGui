using Butler.Interfaces;
using System;

namespace Butler.Implementations
{
    public class LineMaster : ILineMaster
    {
        public GameLine GetLine(
            DateTime gameDate,
            string homeTeamCode)
        {
            var result = new GameLine();
            return result;
        }
    }
}
