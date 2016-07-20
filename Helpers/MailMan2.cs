using Helpers.Interfaces;
using NLog;
using System.Collections.Generic;
using System.Net.Mail;

namespace Helpers
{
   public class MailMan2 : IMailMan
   {
      private const string K_MailServerKey = "MailServer";
      private const string K_MailUsername = "MailUsername";
      private const string K_MailPassword = "MailPassword";

      public Logger Logger { get; set; }

      public string MailServer { get; set; }
      private string UserId { get; set; }
      private string Password { get; set; }

      public SmtpClient SmtpClient { get; set; }

      private List<string> Recipients { get; set; }

      private IConfigReader ConfigReader { get; set; }

      public MailMan2(IConfigReader configReader)
      {
         ConfigReader = configReader;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         var result = Initialise();
      }

      private string Initialise()
      {
         var result = GetMailSettings();
         if ( !string.IsNullOrEmpty( result ) )
            return result;

         SmtpClient = new SmtpClient(MailServer);
         SmtpClient.Port = 465;
         SmtpClient.UseDefaultCredentials = false;   
         SmtpClient.EnableSsl = true;
         SmtpClient.Credentials = new System.Net.NetworkCredential(
            userName:UserId, 
            password:Password);
         Recipients = new List<string>();
         return string.Empty;
      }

      private string GetMailSettings()
      {
         MailServer = ConfigReader.GetSetting( K_MailServerKey );
         if ( string.IsNullOrEmpty( MailServer ) )
            return string.Format( "Failed to read MailServer setting : {0}", K_MailServerKey );
         UserId = ConfigReader.GetSetting( K_MailUsername );
         if ( string.IsNullOrEmpty( UserId ) )
            return string.Format( "Failed to read MailServer setting : {0}", K_MailUsername );
         Password = ConfigReader.GetSetting( K_MailPassword );
         if ( string.IsNullOrEmpty( Password ) )
            return string.Format( "Failed to read MailServer setting : {0}", K_MailPassword );
         return string.Empty;
      }

      public int RecipientCount()
      {
         return Recipients.Count;
      }

      public MailMan2(List<string> recipients)
      {
         Initialise();
         AddRecipients(recipients);
      }

      public void AddRecipients( List<string> recipients )
      {
         Recipients.AddRange( recipients );
      }

      public void AddRecipients( string recipients )
      {
         var recipientArray = recipients.Split( ',' );
         foreach ( var item in recipientArray )
         {
            Recipients.Add( item );
         }
      }

      public string SendMail(string message, string subject)
      {
         try
         {
            using ( var mail = new MailMessage() )
            {
               mail.From = new MailAddress("quarterback16@iinet.net.au");
               foreach (var recipient in Recipients)
               {
                  mail.To.Add(recipient);            
               }
               mail.Subject = subject;
               SmtpClient.Send( mail );
               Logger.Info( "    mail sent to {0}", mail.To );
            }
         }
         catch ( SmtpException ex )
         {
            return ex.Message;
         }

         return string.Empty;
      }

      public string SendMail(string message, string subject, string attachment)
      {
         string[] attachments = new string[1];
         attachments[0] = attachment;
         return SendMail(message, subject, attachments);
      }

      public string SendMail(string message, string subject, string[] attachments)
      {
         try
         {
            using ( var mail = new MailMessage() )
            {
               mail.From = new MailAddress( "quarterback16@grapevine.com.au" );
               foreach ( var recipient in Recipients )
               {
                  mail.To.Add( recipient );
               }
               mail.Subject = subject;
               foreach ( string attachment in attachments )
               {
                  mail.Attachments.Add( new Attachment( attachment ) );
               }
               SmtpClient.Send( mail );
               Logger.Info( "    mail sent to {0}", mail.To );
            }
         }
         catch ( SmtpException ex )
         {
            return ex.Message;
         }

         return string.Empty;
      }

   }
}
