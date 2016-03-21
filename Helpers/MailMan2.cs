using Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Helpers
{
   public class MailMan2 : IMailMan
   {
      public string MailServer { get; set; }

      public SmtpClient SmtpClient { get; set; }

      public MailMan2()
      {
         MailServer = "mail.iinet.net.au";
         SmtpClient = new SmtpClient(MailServer);
         SmtpClient.Port = 465;
         //SmtpClient.UseDefaultCredentials = true;  //  works off Delooch
         SmtpClient.UseDefaultCredentials = false;   //  force authentication?
         SmtpClient.EnableSsl = true;
         SmtpClient.Credentials = new System.Net.NetworkCredential("quarterback16@iinet.net.au", "Brisbane59!");
      }

      public string SendMail(string message, string subject)
      {
         var mail = CreateMailMessage(message,subject);
         mail.From = new MailAddress("quarterback16@iinet.net.au");
#if DEBUG
         mail.To.Add("quarterback16@live.com.au");
#else
         mail.To.Add("stephen.colonna@employment.gov.au");
#endif
         mail.Subject = subject;
         return SendSmtpMail(mail);
      }

      public string SendMail(string message, string subject, string attachment)
      {
         string[] attachments = new string[1];
         attachments[0] = attachment;
         return SendMail(message, subject, attachments);
      }

      public string SendMail(string message, string subject, string[] attachments)
      {
         var mail = CreateMailMessage(message, subject);
         foreach (string attachment in attachments)
         {
            mail.Attachments.Add(new Attachment(attachment));
         }
         return SendSmtpMail(mail);
      }

      private string SendSmtpMail(MailMessage mail)
      {
         try
         {
            SmtpClient.Send(mail);
         }
         catch (SmtpException ex)
         {
            return ex.Message;
         }
         return string.Empty;
      }


      private static MailMessage CreateMailMessage(string message, string subject)
      {
         var mail = new MailMessage();
         mail.From = new MailAddress("quarterback16@grapevine.com.au");
         mail.To.Add("quarterback16@live.com.au");
         mail.To.Add("stephen.colonna@employment.gov.au");
         mail.Subject = subject;
         return mail;
      }
   }
}
