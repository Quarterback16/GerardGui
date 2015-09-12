using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
   public interface IPgmMaster
   {
      PlayerGameMetrics GetPgm(string playerCode, string gameCode );

      void PutPgm(PlayerGameMetrics pgm);
   }
}
