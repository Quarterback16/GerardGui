using System;
using System.Collections.Generic;
using System.IO;
using RosterLib;
using Butler.Helpers;  // for the Last() string extension

namespace Butler.Models
{
   public class LogCleanupJob : Job
   {
      public List<string> LogDirectories { get; set; }

      public int DaysOld { get; set; }

      public int LogsDeleted { get; set; }


      public LogCleanupJob()
      {
         Name = "Log Cleanup";
         Logger = NLog.LogManager.GetCurrentClassLogger();
         LogDirectories = new List<string>();
         DaysOld = 7;  //  TODO: make this a config setting
      }

      public override string DoJob()
      {
         LogsDeleted = 0;

#if DEBUG2
         if ( Logger.IsTraceEnabled )
            Logger.Trace( "Trace Enabled" );
         foreach ( var item in NLog.LogManager.Configuration.LoggingRules )
         {
            Console.WriteLine( "{0}-{1} Logging enabled {2}", item,
               NLog.LogLevel.Trace, item.IsLoggingEnabledForLevel( NLog.LogLevel.Trace ) );
         }
#endif
         var logDirsLoaded = LoadLogDirectoriesFromConfig();
         Logger.Trace( logDirsLoaded );
         //  use the IGetLogDirectories object to find em

         foreach ( var folder in LogDirectories )
         {
            var files = Directory.GetFileSystemEntries( folder );
            var fileCount = 0;
            foreach ( var file in files )
            {
               fileCount++;
               var fileDate = FileUtility.DateOf( file );
               var isTargetted = "OK";
               if ( IsTargetted( file ) )
               {
                  isTargetted = "Targetted!";
                  if (FileUtility.TryToDelete(file))
                  {
                     LogsDeleted++;
                     Logger.Info("  Deleted - {0}", file);
                  }
               }
               Logger.Trace( "{0,2} - {1} - {2} date:{3}", fileCount, file, isTargetted, fileDate  );
            }
         }
         var finishedMessage = string.Format("  {0} job - done. {1} logs deleted", Name, LogsDeleted);
         Logger.Info(finishedMessage);
         return finishedMessage;
      }

      private bool IsTargetted( string file )
      {
         if ( file.Last( 4 ).ToUpper() != ".LOG" )
            return false;
         var theDate = FileUtility.DateOf( file );
         if ( theDate.AddDays( DaysOld ) > DateTime.Now )
            return false;

         return true;
      }

      public override bool IsTimeTodo( out string whyNot )
      {
         base.IsTimeTodo( out whyNot );
         //  do it every time, cant hurt
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return ( string.IsNullOrEmpty( whyNot ) );
      }

      public string LoadLogDirectoriesFromConfig()
      {
         var logDirCount = 0;
         foreach ( string key in System.Configuration.ConfigurationManager.AppSettings )
         {
            var logDirCandidate = key;
            Logger.Trace( "   {0}={1}", key, logDirCandidate );
            if ( IsLogFile( logDirCandidate ) )
            {
               LogDirectories.Add( System.Configuration.ConfigurationManager.AppSettings[ logDirCandidate ].ToString() );
               logDirCount++;
            }
         }
         return string.Format( "{0} Log Directories found in config", logDirCount );
      }

      public static bool IsLogFile( string logDirCandidate )
      {
         if ( logDirCandidate.Length > 12 )
         {
            if ( logDirCandidate.Substring( 0, 13 ) == "log-directory" )
               return true;
         }
         return false;
      }

   }
}
