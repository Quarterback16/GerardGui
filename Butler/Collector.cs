using Helpers.Models;
using RosterLib;
using System;
using System.IO;
using System.Linq;

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

        public static string GetTvFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(
                nameof(TvFolder));
        }

        public static string GetShadowTvFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(
                nameof(ShadowTvFolder));
        }

        public static string GetMovieFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(
                nameof(MovieFolder));
        }

        public static string GetShadowMovieFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(
                nameof(ShadowMovieFolder));
        }

        public static string GetNflFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(
                nameof(NflFolder));
        }

        public static string GetSoccerFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(
                nameof(SoccerFolder));
        }

        public static string GetViewQueueFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(
                nameof(ViewQueueFolder));
        }

        public static string GetMagazineFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(
                nameof(MagazineFolder));
        }

        public static string GetMagazineDestinationFolder()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(
                nameof(MagazineDestinationFolder));
        }

        public bool HaveIt(MediaInfo mi)
        {
            if (TvCollection == null) LoadTvCollection();
            return TvCollection != null 
                && TvCollection.Any(show => mi.Title.ToUpper().Equals(show.ToUpper()));
        }

        public void LoadTvCollection()
        {
            if (!string.IsNullOrEmpty(TvFolder))
            {
                TvCollection = Directory.GetDirectories(TvFolder);
                for (var i = 0; i < TvCollection.Length; i++)
                    TvCollection[i] = TvCollection[i].Substring(TvFolder.Length);

                OutputToConsole();
            }
        }

        private void OutputToConsole()
        {
            var i = 0;
            foreach (var show in TvCollection)
            {
                i++;
                Console.WriteLine($"{i} - {show}");
            }
        }

        public string AddToTvCollection(MediaInfo mi)
        {
            var targetFile = CopyTvTo(mi, TvFolder);
            if (!string.IsNullOrEmpty(ShadowTvFolder))
                CopyMediaTo(mi, ShadowTvFolder);
            return targetFile;
        }

        private string CopyTvTo(MediaInfo mi, string folder)
        {
            if (string.IsNullOrEmpty(folder)) return "No destination folder defined";

            var fromFile = mi.Info.FullName;  //  from DL dir
            var targetFile = $"{folder}{mi.Title}\\Season {mi.Season:0#}\\{mi.Info.Name}";  // to video root
            LatestAddition = targetFile;
            return CopyIt(
                targetFile, 
                fromFile, 
                mi.Info.Name);
        }

        public string AddToMovieCollection(MediaInfo mi)
        {
            var targetFile = CopyMediaTo(mi, MovieFolder);
            if (!string.IsNullOrEmpty(ShadowMovieFolder))
                CopyMediaTo(mi, ShadowMovieFolder);
            return targetFile;
        }

        private static string CopyMediaTo(MediaInfo mi, string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return "No destination folder defined";

            var fromFile = mi.Info.FullName;  //  from DL dir
            var targetFile = $"{folder}{mi.Title}\\{mi.Info.Name}";  // to video root
            return CopyIt(targetFile, fromFile, mi.Info.Name);
        }

        public string AddToSoccerCollection(MediaInfo mi)
        {
            var fromFile = mi.Info.FullName;  //  from DL dir

            if (string.IsNullOrEmpty(SoccerFolder))
                return "No Soccer folder defined";

            var targetFile = $"{SoccerFolder}{mi.Title}";  // to video root
            return CopyIt(targetFile, fromFile, mi.Info.Name);
        }

        public string AddToNflCollection(MediaInfo mi)
        {
            var fromFile = mi.Info.FullName;  //  from DL dir

            if (string.IsNullOrEmpty(NflFolder))
                return "No NFL folder defined";

            var targetFile = $"{NflFolder}{mi.Title}";  // to video root
            return CopyIt(
                targetFile, 
                fromFile, 
                mi.Info.Name);
        }

        public string MoveToMagQueue(MediaInfo mi)
        {
            try
            {
                var fromFile = mi.Info.FullName;
                var targetFile = $"{MagazineDestinationFolder}{mi.Info.Name}";
                return CopyIt(targetFile, fromFile, mi.Info.Name);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"{mi} - {ex.Message}");
                return string.Empty;
            }
        }

        public string MoveToViewQueue(MediaInfo mi)
        {
            try
            {
                var fromFile = mi.Info.FullName;
                var targetFile = $"{ViewQueueFolder}{mi.Info.Name}";
                return CopyIt(targetFile, fromFile, mi.Info.Name);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"{mi} - {ex.Message}");
                return string.Empty;
            }
        }

        private static string CopyIt(
            string targetFile, 
            string fromFile,
            string name)
        {
            if (File.Exists(targetFile))
                targetFile = "Got It - " + name;
            else
            {
                Utility.CopyFile(fromFile, targetFile);
                targetFile = " *NEW* - " + name;
            }
            return targetFile;
        }
    }
}