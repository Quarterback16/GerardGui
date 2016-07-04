namespace Helpers.Interfaces
{
   public interface IMailMan
   {
      string SendMail( string message, string subject );
      string SendMail(string message, string subject, string attachment );
      string SendMail(string message, string subject, string[] attachments);

   }
}
