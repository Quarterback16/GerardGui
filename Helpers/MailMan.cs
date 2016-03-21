using System;
using System.IO;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using Helpers.Interfaces;

namespace Helpers
{
   public class MailMan : IMailMan
   {
      public string SendMail( string message, string subject )
      {
         var client = CreateSmtpClient();
         var mail = CreateMailMessage( message, subject );
         client.Send( mail );
         return string.Empty;
      }

      public string SendMail(string message, string subject, string attachment )
      {
         string[] attachments = new string[1];
         attachments[0] = attachment;
         SendMail(message, subject, attachments);
         return string.Empty;
      }

      public string SendMail(string message, string subject, string[] attachments )
      {
         var client = CreateSmtpClient();
         var mail = CreateMailMessage(message, subject);
         foreach ( string attachment in attachments )
         {
            mail.Attachments.Add( new Attachment( attachment ) );
         }
         client.Send(mail);
         return string.Empty;
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
         var mail = new MailMessage(from: "quarterback16@grapevine.com.au", to: "quarterback16@grapevine.com.au");
         mail.Subject = subject;
         mail.Body = message;
         return mail;
      }

      public static void PokeServer(string server, int port)
      {
         using (var client = new TcpClient())
         {
            client.Connect(server, port);
            // As GMail requires SSL we should use SslStream
            // If your SMTP server doesn't support SSL you can
            // work directly with the underlying stream
            using (var stream = client.GetStream())
            using (var sslStream = new SslStream(stream))
            {
               sslStream.AuthenticateAsClient(server);
               using (var writer = new StreamWriter(sslStream))
               using (var reader = new StreamReader(sslStream))
               {
                  writer.WriteLine("EHLO " + server);
                  writer.Flush();
                  Console.WriteLine(reader.ReadLine());
                  // GMail responds with: 220 mx.google.com ESMTP
               }
            }
         }
      }
   }
}