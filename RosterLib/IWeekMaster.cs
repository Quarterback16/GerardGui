using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
   public interface IWeekMaster
   {
      NFLWeek GetWeek(string season, int week);
      NFLWeek PreviousWeek(NFLWeek theWeek);
   }


}
