using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
    public class CopyJob : Job
    {
        public string SourceDir { get; set; }

        public string DestDir { get; set; }

        public CopyJob(
            string jobName,
            string sourceDir,  //  "d:\\shares\\public\\dropbox\\MediaLists\\"
            string destDir)   //  \\\\DeLooch\\users\\steve\\dropbox\\lists\\
        {
            Name = jobName;
            SourceDir = sourceDir;
            DestDir = destDir;
            Logger = NLog.LogManager.GetCurrentClassLogger();
        }

        public override string DoJob()
        {
            var outcome = FileUtility.CopyDirectory(SourceDir, DestDir, Logger);
            if (string.IsNullOrEmpty(outcome))
            {
                var finishMessage = $"Copied {SourceDir} to {DestDir}";
                Logger.Info("  {0}", finishMessage);
                return finishMessage;
            }

            Logger.Error(outcome);
            return outcome;
        }

        public override bool IsTimeTodo(out string whyNot)
        {
            base.IsTimeTodo(out whyNot);

            if (!string.IsNullOrEmpty(whyNot))
                Logger.Info("Skipped {1}: {0}", whyNot, Name);

            return (string.IsNullOrEmpty(whyNot));
        }
    }
}
