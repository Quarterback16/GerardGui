using NLog;
using System.IO;

namespace Helpers
{
   public class Mover
   {
      public Logger Logger { get; set; }

      public string  WatchFolder { get; set; }

      public Mover(string watchFolder)
      {
         Logger = LogManager.GetCurrentClassLogger();
         WatchFolder = watchFolder;
      }

      public int MoveAllFiles()
      {
         var moved = 0;
         //  read the source directory
         var fileEntries = Directory.GetFiles( SourceDirectory(), "*.torrent" );
         foreach ( var fileName in fileEntries )
         {
            if ( FileAlreadyMoved( fileName ) ) continue;
            MoveIt( fileName );
            moved++;
         }
         return moved;
      }

      public void MoveIt( string fileName )
      {
         CopyFile( fileName, TargetFile( fileName ) );
         CopyFile( fileName, VesuviusFile( fileName ) );
      }

      private static string TargetFile( string fileName )
      {
         var filePart = Path.GetFileName( fileName );
         return string.Format( "{0}{1}", TargetDirectory(), filePart );
      }

      private string VesuviusFile( string fileName )
      {
         var filePart = Path.GetFileName( fileName );
         return string.Format( "{1}{0}", filePart, WatchFolder );
      }

      private static bool FileAlreadyMoved( string fileName )
      {
         return File.Exists( TargetFile( fileName ) );
      }

      public void CopyFile( string sourceFile, string destFile )
      {
         File.SetAttributes(sourceFile, FileAttributes.Normal);
         File.Copy( sourceFile, destFile, true );
         //Logger.Trace( string.Format( "Copied from {0} to {1}", sourceFile, destFile ));
      }

      private static string TargetDirectory()
      {
         return ".\\Moved\\";
      }

      private static string SourceDirectory()
      {
         return ".\\Source\\";
      }
   }
}