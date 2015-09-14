using System.Net.Mail;

namespace Helpers
{
   public static class MailMan
   {
      public static void SendMail( string message, string subject )
      {
         var client = CreateSmtpClient();
         var mail = CreateMailMessage( message, subject );
         client.Send( mail );
      }

      public static void SendMail(string message, string subject, string attachment )
      {
         string[] attachments = new string[1];
         attachments[0] = attachment;
         SendMail(message, subject, attachments);
      }

      public static void SendMail(string message, string subject, string[] attachments )
      {
         var client = CreateSmtpClient();
         var mail = CreateMailMessage(message, subject);
         foreach ( string attachment in attachments )
         {
            mail.Attachments.Add( new Attachment( attachment ) );
         }
         client.Send(mail);
      }

      private static SmtpClient CreateSmtpClient()
      {
         var client = new SmtpClient
         {
            Port = 25,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = true,
            Host = "192.168.1.113"   //  Regina's IP address locally
         };
         return client;
      }

      private static MailMessage CreateMailMessage(string message, string subject )
      {
         var mail = new MailMessage(from: "quarterback16@grapevine.com.au", to: "stephen.colonna@employment.gov.au");
         mail.Subject = subject;
         mail.Body = message;
         return mail;
      }
   }
}