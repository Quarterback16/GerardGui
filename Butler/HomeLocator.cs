using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace Butler
{

    public class HomeLocator
    {
        public Logger Logger { get; set; }
        public string HomeFolder { get; set; }
        public string FolderPath { get; set; }
        public List<string> ITFolderCollection { get; set; }

        public HomeLocator( string homeFolder )
        {
            HomeFolder = homeFolder;
            Log($"HomeFolder: {HomeFolder}");
            LoadITFolderCollection();
        }

        private void Log(string message)
        {
            if (Logger != null)
            {
                Logger.Info(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        public HomeLocator() : this(@"\\\\Regina\books\")
        {
        }

        public HomeLocator(Logger logger) : this(@"\\\\Regina\books\")
        {
            Logger = logger;
        }

        public string HomeFor( string fileName )
        {
            var fileNameNoExt = Path.GetFileNameWithoutExtension(fileName);
            const string category = "IT";
            var home = string.Empty;
            foreach (var candidate in ITFolderCollection)
            {
                if (fileNameNoExt.ToUpper().Contains(candidate.ToUpper()))
                    return HomePath( category, candidate, fileName);
            }
            //  No hit on IT, try hard coded special cases
            if (RelatesToCooking(fileName))
                return HomePath("Cooking",fileName);

            if (RelatesToHomeCare(fileName))
                return HomePath("Home Care", fileName);

            var specialCategory = "Sherlock Holmes";
            if (RelatesTo(specialCategory, fileName))
                return HomePath(specialCategory, fileName);

            specialCategory = "Kama Sutra";
            if (RelatesTo(specialCategory, fileName))
                return HomePath(specialCategory, fileName);

            specialCategory = "Physics";
            if (RelatesTo(specialCategory, fileName))
                return HomePath(specialCategory, fileName);

            specialCategory = "Probability";
            if (RelatesTo(specialCategory, fileName))
                return HomePath(specialCategory, fileName);

            specialCategory = "Program Delivery";
            if (RelatesTo(specialCategory, fileName))
                return HomePath(specialCategory, fileName);

            specialCategory = "Apartment Buildings";
            if (RelatesTo(specialCategory, fileName))
                return HomePath(specialCategory, fileName);

            specialCategory = "Motivation";
            if (RelatesTo(specialCategory, fileName))
                return HomePath(specialCategory, fileName);

            specialCategory = "Abs";
            if (RelatesTo(specialCategory, fileName))
                return HomePath(specialCategory, fileName);

            specialCategory = "Writing";
            if (RelatesTo(specialCategory, fileName))
                return HomePath(specialCategory, fileName);
            return home;
        }

        private static bool RelatesToHomeCare(string fileName)
        {
            if (fileName.ToUpper().Contains("TIDY")
                || fileName.ToUpper().Contains("HOME"))
                return true;
            return false;
        }

        private static bool RelatesTo(
            string specialCategory,
            string fileName)
        {
            if (fileName.ToUpper().Contains(specialCategory.ToUpper()))
                return true;
            return false;
        }

        private string HomePath(
            string category,
            string candidate,
            string fileName)
        {
            FolderPath = $@"{HomeFolder}{category}\{candidate}";
            return $@"{FolderPath}\{fileName}";
        }

        private string HomePath(
            string category,
            string fileName)
        {
            FolderPath = $@"{HomeFolder}{category}";
            return $@"{FolderPath}\{fileName}";
        }

        private static bool RelatesToCooking(string fileName)
        {
            if (fileName.ToUpper().Contains("NIGELLA")
                || fileName.ToUpper().Contains("COOKING")
                || fileName.ToUpper().Contains("EATING"))
                return true;
            return false;
        }

        private void LoadITFolderCollection()
        {
            ITFolderCollection = new List<string>();
            if (!string.IsNullOrEmpty(HomeFolder))
            {
                var directories = Directory.GetDirectories(
                    HomeFolder+@"IT\");
                foreach (var pathName in directories)
                {
                    var stem = Path.GetFileName(pathName);
                    if (stem.Length > 1)
                        ITFolderCollection.Add(stem);
                }
                var lc = new LengthComparer();
                ITFolderCollection.Sort(lc);
                DumpFolderCollection();
            }
        }

        private void DumpFolderCollection()
        {
            var i = 0;
            foreach (var folder in ITFolderCollection)
            {
                i++;
                Log($"{i} - {folder}");
            }
        }
    }

    public class LengthComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x.Length > y.Length)
                return -1;
            if (x.Length < y.Length)
                return 0;
            return 1;
        }
    }
}
