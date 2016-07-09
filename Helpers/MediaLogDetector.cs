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
            if (MediaFileMatches(logType, file, logDate))
            {
               fileList.Add(file);
            }
            else
            {
               Logger.Trace( "    {0} Does not match", file );
            }
         }
         return fileList;
      }

      public string FilePartFile(string dir, string file)
      {
         var fileInfo = new System.IO.FileInfo(file);
         return fileInfo.Name;
      }

      public bool MediaFileMatches( string logType, string fileName, DateTime logDate )
      {
         var isMatch = false;

         if ( FileNameWithNoExtension(fileName).StartsWith( logType ) )
         {
            var fileDate = MediaFileDate( fileName );
            if ( fileDate.Date > logDate.Date )
            {
               isMatch = true;
            }
            else
            {
               //Logger.Info( "  file {0} is older than the last logdate of {1:d}", fileNameWithNoExtension, logDate.Date );
            }
         }

         return isMatch;
      }

      private string FileNameWithNoExtension(string fileName)
      {
         var fi = new System.IO.FileInfo( fileName );
         return fi.Name;
      }

      public DateTime MediaFileDate( string fileName )
      {
         Logger.Info( "Looking for file called {0}", fileName );
         var fi = new System.IO.FileInfo( fileName );
         var dateOfFile = fi.LastWriteTime.ToString();
         Logger.Trace( "  Update Date of {0} is {1}", fi.Name, dateOfFile );
         var fileDate = new DateTime( 1, 1, 1 );
         if ( DateTime.TryParse( dateOfFile, out fileDate ) )
            return fileDate;
         else
         {
            Logger.Error( "filedate {0} did not parse", dateOfFile );
            return new DateTime( 1, 1, 1 );
         }
      }
   }
}
