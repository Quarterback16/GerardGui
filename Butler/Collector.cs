using System;
using System.IO;
using System.Linq;
using Butler.Models;
using RosterLib;
using Helpers.Models;

namespace Butler
{
	public class Collector
	{
		public string[] TvCollection { get; set; }

		public string TvFolder { get; set; }
      public string ShadowTvFolder { get; set; }

      public string MovieFolder { get; set; }
      public string ShadowMovieFolder { get; set; }

      public string NflFolder { get; set; }

      public string SoccerFolder { get; set; }

		public string ViewQueueFolder { get; set; }

      public string MagazineFolder { get; set; }

      public string MagazineDestinationFolder { get; set; }

		public string LatestAddition { get; set; }

		public Collector()
		{
         TvFolder = GetTvFolder();     //   @"\\Regina\video\TV\";
         ShadowTvFolder = GetShadowTvFolder(); 
         MovieFolder = GetMovieFolder();
         ShadowMovieFolder = GetShadowMovieFolder();
         ViewQueueFolder = GetViewQueueFolder();  //  @"\\Vesuvius\books\View Queue\";
         MagazineFolder = GetMagazineFolder();
         MagazineDestinationFolder = GetMagazineDestinationFolder(); 
         NflFolder = GetNflFolder();
         SoccerFolder = GetSoccerFolder();
		}

      public string GetTvFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get( "TvFolder" );
      }

      public string GetShadowTvFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get( "ShadowTvFolder" );
      }

      public string GetMovieFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get( "MovieFolder" );
      }
      public string GetShadowMovieFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get( "ShadowMovieFolder" );
      }

      public string GetNflFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get( "NflFolder" );
      }

      public string GetSoccerFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get( "SoccerFolder" );
      }

      public string GetViewQueueFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get( "ViewQueueFolder" );
      }

      public string GetMagazineFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get("MagazineFolder");
      }

      public string GetMagazineDestinationFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get("MagazineDestinationFolder");
      }

		public bool HaveIt( MediaInfo mi )
		{
			if ( TvCollection == null ) LoadTvCollection();
			return TvCollection != null && TvCollection.Any( show => mi.Title.ToUpper().Equals( show.ToUpper() ) );
		}

		public void LoadTvCollection()
		{
         if ( ! string.IsNullOrEmpty( TvFolder ) )
         {
            TvCollection = Directory.GetDirectories( TvFolder );
            for ( var i = 0; i < TvCollection.Length; i++ )
               TvCollection[ i ] = TvCollection[ i ].Substring( TvFolder.Length );

            OutputToConsole();
         }
		}

		private void OutputToConsole()
		{
			var i = 0;
			foreach ( var show in TvCollection )
			{
				i++;
				Console.WriteLine( "{0} - {1}", i, show );
			}
		}

		public string AddToTvCollection( MediaInfo mi)
		{
         var targetFile = CopyTvTo( mi, TvFolder );
         if ( !string.IsNullOrEmpty( ShadowTvFolder ) )
            CopyMediaTo( mi, ShadowTvFolder );
         return targetFile;
		}

      private string CopyTvTo( MediaInfo mi, string folder )
      {
         if ( string.IsNullOrEmpty( folder ) ) return "No destination folder defined";

         var fromFile = mi.Info.FullName;  //  from DL dir
         var targetFile = string.Format( "{0}{1}\\Season {3:0#}\\{2}",
             folder, mi.Title, mi.Info.Name, mi.Season );  // to video root
         LatestAddition = targetFile;

         return CopyIt( targetFile, fromFile, mi.Info.Name );
      }


      public string AddToMovieCollection( MediaInfo mi )
      {
         var targetFile = CopyMediaTo( mi, MovieFolder );
         if ( ! string.IsNullOrEmpty( ShadowMovieFolder ) )
            CopyMediaTo( mi, ShadowMovieFolder );
         return targetFile;
      }

      private static string CopyMediaTo( MediaInfo mi, string folder )
      {
         if ( string.IsNullOrEmpty( folder ) ) return "No destination folder defined";

         var fromFile = mi.Info.FullName;  //  from DL dir
         var targetFile = string.Format( "{0}{1}\\{2}",
             folder, mi.Title, mi.Info.Name  );  // to video root
         return CopyIt( targetFile, fromFile, mi.Info.Name );
      }

      public string AddToSoccerCollection( MediaInfo mi )
      {
         var fromFile = mi.Info.FullName;  //  from DL dir

         if ( string.IsNullOrEmpty( SoccerFolder ) ) return "No Soccer folder defined";

         var targetFile = string.Format( "{0}{1}",
             SoccerFolder, mi.Title  );  // to video root
         return CopyIt( targetFile, fromFile, mi.Info.Name );
      }

      public string AddToNflCollection( MediaInfo mi )
      {
         var fromFile = mi.Info.FullName;  //  from DL dir

         if ( string.IsNullOrEmpty( NflFolder ) ) return "No NFL folder defined";

         var targetFile = string.Format( "{0}{1}",
             NflFolder, mi.Title  );  // to video root
         return CopyIt( targetFile, fromFile, mi.Info.Name );
      }

      public string MoveToMagQueue(MediaInfo mi)
      {
         try
         {
            var fromFile = mi.Info.FullName;
            var targetFile = string.Format("{0}{1}", MagazineDestinationFolder, mi.Info.Name);
            return CopyIt(targetFile, fromFile, mi.Info.Name);
         }
         catch (UnauthorizedAccessException ex)
         {
            Console.WriteLine("{0} - {1}", mi, ex.Message);
            return string.Empty;
         }
      }

		public string MoveToViewQueue( MediaInfo mi )
		{
         try
         {
            var fromFile = mi.Info.FullName;
            var targetFile = string.Format("{0}{1}", ViewQueueFolder, mi.Info.Name);
            return CopyIt( targetFile, fromFile, mi.Info.Name );
         }
         catch ( UnauthorizedAccessException ex )
         {
            Console.WriteLine("{0} - {1}", mi, ex.Message );
            return string.Empty;
         }
		}

	   private static string CopyIt( string targetFile, string fromFile, string name )
	   {
	      if (File.Exists( targetFile ))
	         targetFile = "Got It - " + name;
	      else
	      {
	         Utility.CopyFile( fromFile, targetFile );
	         targetFile = " *NEW* - " + name;
	      }
	      return targetFile;
	   }
	}
}