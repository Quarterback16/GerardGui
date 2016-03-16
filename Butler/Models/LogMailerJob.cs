using Helpers;
using Helpers.Interfaces;
using Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Butler.Models
{
   public class LogMailerJob : Job
   {
      public int LogsMailed { get; set; }

      protected IMailMan MailMan { get; set; }

      protected IDetectLogFiles LogFileDetector { get; set; }

      protected LogMaster LogMaster { get; set; }

      public LogMailerJob(IMailMan mailMan, IDetectLogFiles logFileDetector)
      {
         Name = "Log Mailer";
         Console.WriteLine("Constructing {0} ...", Name);
         Logger = NLog.LogManager.GetCurrentClassLogger();
         LogsMailed = 0;
         MailMan = mailMan;
         LogMaster = new LogMaster(".\\xml\\mail-list.xml");
         LogFileDetector = logFileDetector;
      }

      public override string DoJob()
      {
         Logger.Info("Doing {0} job..............................................", Name);

         var myEnumerator = LogMaster.TheHT.GetEnumerator();
         while (myEnumerator.MoveNext())
         {
            // for each designated logfile
            var logitem = (LogItem) myEnumerator.Current;

            MailLogFiles(logitem);
         }

         var finishedMessage = string.Format("  {0} job - done. {1} logs mailed", Name, LogsMailed);
         Logger.Info(finishedMessage);
         return finishedMessage;
      }

      private void MailLogFiles(LogItem logitem)
      {
         var filesFound = LogFileDetector.DetectLogFileIn(logitem.LogDir, logitem.Filespec, logitem.MailDate);
         foreach (var file in filesFound)
         {
            MailMan.SendMail(message: "Log file", subject: "For perusal", attachment:file);
            LogsMailed++;
         }
      }

   }
}
