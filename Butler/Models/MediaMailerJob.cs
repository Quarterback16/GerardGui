using Helpers;
using Helpers.Interfaces;
using Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Butler.Models
{
   public class MediaMailerJob : Job
   {
      public int LogsMailed { get; set; }

      protected IMailMan MailMan { get; set; }

      protected IDetectLogFiles LogFileDetector { get; set; }

      protected LogMaster LogMaster { get; set; }

      public MediaMailerJob(IMailMan mailMan, IDetectLogFiles logFileDetector)
      {
         Name = "Media Mailer";
         Logger = NLog.LogManager.GetCurrentClassLogger();
         MailMan = mailMan;
         LogMaster = new LogMaster(".\\xml\\media-mail-list.xml");
         LogFileDetector = logFileDetector;
      }

      public override string DoJob()
      {
         LogsMailed = 0; 
         
         var lastDate = new DateTime(1, 1, 1);

         // load files meta data ur interested in
         List<string> keys = new List<string>();
         foreach (System.Collections.DictionaryEntry de in LogMaster.TheHT)
            keys.Add(de.Key.ToString());

         foreach (string key in keys)
         {
            var logitem = (LogItem)LogMaster.TheHT[key];
            lastDate = MailLogFiles(logitem);
            if (lastDate != new DateTime(1, 1, 1))
            {
               logitem.MailDate = lastDate;
               LogMaster.PutItem(logitem);
            }
         }

         LogMaster.Dump2Xml();  //  write changes if any

         var finishedMessage = string.Format("  {0} job - done. {1} logs mailed", Name, LogsMailed);
         Logger.Info(finishedMessage);
         return finishedMessage;
      }

      private DateTime MailLogFiles(LogItem logitem)
      {
         var lastDate = new DateTime(1, 1, 1);
         var filesFound = LogFileDetector.DetectLogFileIn(logitem.LogDir, logitem.Filespec, logitem.MailDate);
         foreach (var file in filesFound)
         {
            var errorMsg = MailMan.SendMail(message: "Recently Added", subject: "New additions", attachment:file);
            if (string.IsNullOrEmpty(errorMsg))
            {
               lastDate = LogFileDetector.FileDate(LogFileDetector.FilePartFile(logitem.LogDir, file));
               LogsMailed++;
               Logger.Info(string.Format("Emailed {0}", file));
            }
            else
            {
               Logger.Error(string.Format("Failed to email {0} - {1}", file, errorMsg));
            }
         }
         return lastDate;
      }
   }
}
