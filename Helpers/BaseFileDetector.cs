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

      public DateTime FileDate(string dir, string file)
      {
         var len = file.Length;
         if ( DateIsNotEmbeddedIntheFileName( len ) )
         {
            //  the date is not embedded in the log file name
            var fi = new System.IO.FileInfo( dir + file );
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
         return DateTime.Parse( file.Substring( len - 10, 10 ) );
      }

      private static bool DateIsNotEmbeddedIntheFileName( int len )
      {
         return len < 21;
      }

      protected bool FileMatches(string dir, string file, string logType, DateTime logDate)
      {
         var isMatch = false;
         if ( file.Length < 10 )
         {
            Logger.Trace( "  file {0} length is too small", file );
            return false;
         }
         if (file.StartsWith(logType))
         {
            var fileDate = FileDate(dir, file);
            if ( fileDate > logDate )
            {
               if ( logDate != DateTime.Now.Date )
               {
                  isMatch = true;
               }
               else
               {
                  Logger.Info( "  file {0} already mailed today", file );
               }
            }
         }
         else
         {
            Logger.Trace( "  file {0} does not start with {1}", file, logType );
         }
         return isMatch;
      }
   }
}
