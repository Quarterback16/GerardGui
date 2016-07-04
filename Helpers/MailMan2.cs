using Helpers.Interfaces;
using NLog;
using System.Collections.Generic;
using System.Net.Mail;

namespace Helpers
{
   public class MailMan2 : IMailMan
   {
      public Logger Logger { get; set; }

      public string MailServer { get; set; }

      public SmtpClient SmtpClient { get; set; }

      private List<string> Recipients { get; set; }

      public MailMan2()
      {
         Initialise();
         Recipients.Add("quarterback16@live.com.au");
         //TODO: Get recipients from Config or perhaps XML file as its just data.  We want to be able to edit recipients wihout changing code.
      }

      private void Initialise()
      {
         Logger = NLog.LogManager.GetCurrentClassLogger();
         MailServer = "mail.iinet.net.au";  //TODO:  move to config
         SmtpClient = new SmtpClient(MailServer);
         SmtpClient.Port = 465;
         SmtpClient.UseDefaultCredentials = false;   //  force authentication?
         SmtpClient.EnableSsl = true;
         SmtpClient.Credentials = new System.Net.NetworkCredential("quarterback16@iinet.net.au", "Brisbane59!"); //TODO:  move to config
         Recipients = new List<string>();
      }

      public MailMan2(List<string> recipients)
      {
         Initialise();
         Recipients.AddRange(recipients);
      }

      public string SendMail(string message, string subject)
      {
         var mail = CreateMailMessage(message,subject);
         mail.From = new MailAddress("quarterback16@iinet.net.au");
         foreach (var recipient in Recipients)
         {
            mail.To.Add(recipient);            
         }
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
            Logger.Info( "    mail sent to {0}", mail.To );
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
