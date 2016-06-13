using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
   public class BaseFileDetector
   {
      public DateTime FileDate(string file)
      {
         var len = file.Length;
         if (len < 21) return DateTime.Now.AddDays(-1);
         return DateTime.Parse(file.Substring(len - 10, 10));
      }

      protected bool FileMatches(string file, string logType, DateTime logDate)
      {
         var isMatch = false;
         if (file.Length < 10) return false;
         if (file.StartsWith(logType))
         {
            var fileDate = FileDate(file);
            if (fileDate > logDate && fileDate.Date != DateTime.Now.Date)
               //  logfile is after the last date mailed and not todays
               isMatch = true;
         }
         return isMatch;
      }
   }
}
