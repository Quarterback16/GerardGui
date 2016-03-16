using Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
   public class LogFileDetector : IDetectLogFiles
   {
      public List<string> DetectLogFileIn(string dir, string logType, DateTime logDate)
      {
         var fileList = new List<string>();
         var filesInDir = System.IO.Directory.GetFiles(dir);
         foreach (var file in filesInDir)
         {
            var filepart = FilePartFile(dir, file);
            if (FileMatches(filepart, logType, logDate))
            {
               fileList.Add(file);
            }
         }
         return fileList;
      }

      private string FilePartFile(string dir, string file)
      {
         var len = file.Length - 4 - dir.Length;
         if (len < 10) return file;
         var filePart = file.Substring(dir.Length, len);
         return filePart;
      }

      private bool FileMatches(string file, string logType, DateTime logDate)
      {
         var isMatch = false;
         if (file.Length < 10) return false;
         if (file.StartsWith( logType ) )
         {
            var len = file.Length;
            var fileDate = DateTime.Parse(file.Substring(len - 10, 10));
            if (fileDate > logDate)
               isMatch = true;
         }
         return isMatch;
      }
   }
}
