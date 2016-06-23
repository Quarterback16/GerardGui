using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
   public class BaseFileDetector
   {
      public DateTime FileDate(string dir, string file)
      {
         var len = file.Length;
         if (len < 21)
         {
            //  the date is not embedded in the log file name
            var fi = new System.IO.FileInfo(dir + file);
            var dateOfFile = fi.CreationTime.ToString();
            return DateTime.Parse(dateOfFile);
         }
         return DateTime.Parse(file.Substring(len - 10, 10));
      }

      protected bool FileMatches(string dir, string file, string logType, DateTime logDate)
      {
         var isMatch = false;
         if (file.Length < 10) return false;
         if (file.StartsWith(logType))
         {
            var fileDate = FileDate(dir, file);
            if (fileDate > logDate && fileDate.Date != DateTime.Now.Date)
               //  logfile is after the last date mailed and not todays active log
               isMatch = true;
         }
         return isMatch;
      }
   }
}
