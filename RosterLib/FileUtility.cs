using System;
using System.IO;
using System.Linq;

namespace RosterLib
{
	public static class FileUtility
	{
		public static DateTime DateOf( string fileName )
		{
			if (!File.Exists( fileName )) 
				return new DateTime( 1, 1, 1 );

			var fileInfo = new FileInfo( fileName );
			return fileInfo.LastWriteTime;
		}

		public static string CopyDirectory( string Src, string Dst )
		{
         try
         {
            if ( Dst[ Dst.Length - 1 ] != Path.DirectorySeparatorChar )
               Dst += Path.DirectorySeparatorChar;
            if ( !Directory.Exists( Dst ) ) Directory.CreateDirectory( Dst );
            string[] Files = Directory.GetFileSystemEntries( Src );
            foreach ( var Element in Files )
            {
               // Sub directories
               if (Directory.Exists(Element))
                  CopyDirectory(Element, Dst + Path.GetFileName(Element));
               // Files in directory
               else
               {
                  //if (!File.Exists(Dst + Path.GetFileName(Element) )) 
                     File.Copy(Element, Dst + Path.GetFileName(Element), true);
               }
            }
            return string.Empty;
         }
         catch ( IOException ex )
         {
            return ex.Message;
         }

		}

		public static bool TryToDelete( string f )
		{
			try
			{
				// A.
				// Try to delete the file.
				File.Delete( f );
				return true;
			}
			catch ( IOException )
			{
				// B.
				// We could not delete the file.
				return false;
			}
		}

		public static void DeleteAllFilesInDirectory(string dir)
		{
			var downloadedMessageInfo = new DirectoryInfo(dir);

			foreach (var file in downloadedMessageInfo.GetFiles())
			{
				file.Delete();
			}
			foreach (var d in downloadedMessageInfo.GetDirectories())
			{
				d.Delete(true);
			}			
		}

		public static int CountFilesInDirectory(string dir)
		{
			var downloadedMessageInfo = new DirectoryInfo(dir);
			var files = downloadedMessageInfo.GetFiles();
			return files.Count();
		}

	}
}