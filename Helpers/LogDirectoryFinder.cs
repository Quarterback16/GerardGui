using Helpers.Interfaces;
using System.Collections.Generic;
using System.Configuration;

namespace Helpers
{
   public class LogDirectoryFinder : IFindLogDirectories
   {
      public List<string> GetLogDirectories()
      {
         var dirList = new List<string>();
         var logDirCount = 0;
         foreach (string key in ConfigurationManager.AppSettings)
         {
            var logDirCandidate = key;
            if (IsLogFile(logDirCandidate))
            {
               dirList.Add(ConfigurationManager.AppSettings[logDirCandidate].ToString());
               logDirCount++;
            }
         }
         return dirList;
      }

      private bool IsLogFile(string logDirCandidate)
      {
         if (logDirCandidate.Length > 12)
         {
            if (logDirCandidate.Substring(0, 13) == "log-directory")
               return true;
         }
         return false;
      }
   }

}
