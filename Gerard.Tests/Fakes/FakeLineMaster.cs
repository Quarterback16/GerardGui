using Butler.Implementations;
using Butler.Interfaces;
using System;

namespace Gerard.Tests.Fakes
{
    public class FakeLineMaster : ILineMaster
    {
        public GameLine GetLine(
            DateTime gameDate,
            string homeTeamCode)
        {
            //var gameLine = new GameLine
            //{
            //    Total = 44.0M,
            //    Spread = 3.0M
            //};
            var gameLine = new GameLine();
            return gameLine;
        }
    }
}
