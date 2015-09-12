using System.Net.Mail;

namespace Helpers
{
   public static class MailMan
   {
      public static void SendMail( string message, string subject )
      {
         var mail = new MailMessage("quarterback16@grapevine.com.au","quarterback16@grapevine.com.au");
         var client = new SmtpClient
         {
            Port = 25,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = true,
            Host = "192.168.1.113"   //  Regina's IP address locally
         };
         mail.Subject = subject;
         mail.Body = message;
         client.Send( mail );
      }
   }
}