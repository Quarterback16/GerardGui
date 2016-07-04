using Helpers.Interfaces;
using System;
using System.Collections.Generic;

namespace Helpers
{
   public class MediaLogDetector : BaseFileDetector, IDetectLogFiles
   {
      public List<string> DetectLogFileIn(string dir, string logType, DateTime logDate)
      {
         var fileList = new List<string>();
         var filesInDir = System.IO.Directory.GetFiles(dir);
         Logger.Trace( "  {0} files found in {1}", filesInDir.Length, dir );
         foreach (var file in filesInDir)
         {
            var filepart = FilePartFile(dir, file);
            if (FileMatches(dir, filepart, logType, logDate))
            {
               fileList.Add(file);
            }
            else
            {
               Logger.Trace( "    {0} Does not match", filepart );
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
