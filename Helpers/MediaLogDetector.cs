using Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
   public class MediaLogDetector : BaseFileDetector, IDetectLogFiles
   {
      public List<string> DetectLogFileIn(string dir, string logType, DateTime logDate)
      {
         var fileList = new List<string>();
         var filesInDir = System.IO.Directory.GetFiles(dir);
         foreach (var file in filesInDir)
         {
            var filepart = FilePartFile(dir, file);
            if (FileMatches(dir, filepart, logType, logDate))
            {
               fileList.Add(file);
            }
         }
         return fileList;
      }

      public string FilePartFile(string dir, string file)
      {
         var fileInfo = new System.IO.FileInfo(file);
         return fileInfo.Name;
      }
   }
}
