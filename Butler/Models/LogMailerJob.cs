﻿using Helpers;
using Helpers.Interfaces;
using Helpers.Models;
using System;
using System.Collections.Generic;

namespace Butler.Models
{
   public class LogMailerJob : Job
   {
      private const string K_RecipientsKey = "LogRecipients";

      public int LogsMailed { get; set; }

      protected IMailMan MailMan { get; set; }

      protected IDetectLogFiles LogFileDetector { get; set; }

      protected LogMaster LogMaster { get; set; }

      public LogMailerJob(IMailMan mailMan, IDetectLogFiles logFileDetector, IConfigReader configReader)
      {
         Name = "Log Mailer";
         Logger = NLog.LogManager.GetCurrentClassLogger();
         MailMan = mailMan;
         InstructMailMan( configReader );
         LogMaster = new LogMaster(".\\xml\\mail-list.xml");
         LogFileDetector = logFileDetector;
      }

      private void InstructMailMan( IConfigReader configReader )
      {
         var recipients = configReader.GetSetting( K_RecipientsKey );
         if ( string.IsNullOrEmpty( recipients ) )
         {
            throw new ApplicationException( "Recipients Key is empty" );
         }
         MailMan.AddRecipients( recipients );
      }

      public override string DoJob()
      {
         LogsMailed = 0;

         var lastDate = new DateTime(1, 1, 1);

         var keys = new List<string>();
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
            var errorMsg = MailMan.SendMail(message: "Log file", subject: "For perusal", attachment:file);
            if (string.IsNullOrEmpty(errorMsg))
            {
               lastDate = LogFileDetector.FileDate(logitem.LogDir, LogFileDetector.FilePartFile(logitem.LogDir, file));
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