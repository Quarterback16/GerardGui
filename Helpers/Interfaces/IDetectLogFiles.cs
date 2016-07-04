using System;
using System.Collections.Generic;

namespace Helpers.Interfaces
{
   public interface IDetectLogFiles
   {
      List<string> DetectLogFileIn(string dir, string logType, DateTime logDate);

      DateTime FileDate(string dir, string file);

      string FilePartFile(string dir, string file);
   }
}
