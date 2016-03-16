using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers.Interfaces
{
   public interface IMailMan
   {
      void SendMail( string message, string subject );
      void SendMail(string message, string subject, string attachment );
      void SendMail(string message, string subject, string[] attachments);

   }
}
