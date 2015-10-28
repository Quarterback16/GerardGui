using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib.Interfaces
{
   public interface IGetGamebooks
   {
      int DownloadWeek(NFLWeek week);

      string Seed(NFLWeek week);
   }
}
