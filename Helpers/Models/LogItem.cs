using System;
using System.Xml;

namespace Helpers.Models
{
   public class LogItem
   {
      private XmlNode node;

      public string LogDir { get; set; }
      public string Filespec { get; set; }
      public DateTime MailDate { get; set; }

		public LogItem(XmlNode node)
		{
			this.node = node;
			foreach ( XmlNode n in node.ChildNodes )
			{
            switch ( n.Name )
            {
               case "log-dir":
                  LogDir = n.InnerText;
                  break;
               case "filespec":
                  Filespec = n.InnerText;
                  break;
					case "datemailed":
						MailDate = DateTime.Parse( n.InnerText );
						break;
               default:
                  break;
            }
			}

		}
   }
}
