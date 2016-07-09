using NLog;
using System;

namespace Helpers
{
   public class BaseFileDetector
   {
      public Logger Logger { get; set; }

      public BaseFileDetector()
      {
         Logger = NLog.LogManager.GetCurrentClassLogger();
      }

      public DateTime FileDate(string dir, string fileNameWithNoExtension)
      {
         var len = fileNameWithNoExtension.Length;
         if ( DateIsNotEmbeddedIntheFileName( len ) )
         {
            //  the date is not embedded in the log file name
            var fi = new System.IO.FileInfo( dir + fileNameWithNoExtension );
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
         return DateTime.Parse( fileNameWithNoExtension.Substring( len - 10, 10 ) );
      }

      private static bool DateIsNotEmbeddedIntheFileName( int len )
      {
         return len < 21;
      }

      public bool FileMatches(string dir, string fileNameWithNoExtension, string logType, DateTime logDate)
      {
         var isMatch = false;
         if ( fileNameWithNoExtension.Length < 10 )
         {
            Logger.Trace( "  file {0} length is too small", fileNameWithNoExtension );
            return false;
         }
         if (fileNameWithNoExtension.StartsWith(logType))
         {
            var fileDate = FileDate(dir, fileNameWithNoExtension);
            if ( fileDate.Date > logDate.Date )
            {
               if ( fileDate.Date == DateTime.Now.Date )
               {
                  Logger.Info( "  file {0} may still be in progress", fileNameWithNoExtension );
               }
               else
               {
                  isMatch = true;
               }
            }
            else
            {
               //Logger.Info( "  file {0} is older than the last logdate of {1:d}", fileNameWithNoExtension, logDate.Date );
            }
         }
         else
         {
            Logger.Trace( "  file {0} does not start with {1}", fileNameWithNoExtension, logType );
         }
         return isMatch;
      }
   }
}
