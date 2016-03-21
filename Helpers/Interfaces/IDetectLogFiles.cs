using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Interfaces
{
   public interface IDetectLogFiles
   {
      List<string> DetectLogFileIn(string dir, string logType, DateTime logDate);

      DateTime FileDate(string file);

      string FilePartFile(string dir, string file);
   }
}
