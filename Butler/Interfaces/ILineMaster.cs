using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Butler.Interfaces
{
    public interface ILineMaster
    {
        GameLine GetLine(
            DateTime gameDate,
            string homeTeamCode);
    }
}
