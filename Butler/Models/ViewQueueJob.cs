using Helpers;
using NLog;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Butler.Models
{
    public class ViewQueueJob : Job
    {
        public List<string> Items { get; set; }

        public string ViewQueueFolder { get; set; }

        public ViewQueueJob(Logger logger)
        {
            Name = "View Queue Job";
            Logger = logger;
            ViewQueueFolder = ConfigurationManager.AppSettings[
                AppSettings.ViewQueueFolder];
        }

        public ViewQueueJob() : this(LogManager.GetCurrentClassLogger())
        {
        }

        public override string DoJob()
        {
            var homeLocator = new HomeLocator(Logger);
            var mover = new Mover();
            GetFiles();
            foreach (var file in Items)
            {
                MoveFileToHomeLocation(homeLocator, mover, file);
            }
            return homeLocator.HomeFolder;
        }

        private void MoveFileToHomeLocation(
            HomeLocator homeLocator,
            Mover mover,
            string item)
        {
            var fileName = Path.GetFileName(item);
            if (IsATestFile(fileName))
                return;

            var newFile = homeLocator.HomeFor(fileName);
            if (!string.IsNullOrEmpty(newFile))
            {
                mover.MoveFile(
                    item,
                    destFile: newFile);
                Logger.Info(
                    $@"   moved file {fileName,20} to {
                        homeLocator.FolderPath
                        }");
            }
        }

        private static bool IsATestFile(string fileName)
        {
            if (fileName == "Bitcoin Explained.epub")
            {
                System.Console.WriteLine($"{fileName,20} is a test file; skipping");
                return true;
            }
            return false;
        }

        public void GetFiles()
        {
            try
            {
                Items = new List<string>();

                Items = Directory.GetFiles(
                    path: ViewQueueFolder,
                    searchPattern: "*.*",
                    searchOption: SearchOption.TopDirectoryOnly).ToList();

                Logger.Info(
                    $"Found {Items.Count} files in folder:{ViewQueueFolder}");
            }
            catch (IOException ex)
            {
                Logger.Error(
                    $"Invalid DL folder {ViewQueueFolder} :{ex.Message}");
            }
        }
    }
}
