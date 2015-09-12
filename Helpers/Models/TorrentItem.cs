using System;
using System.Xml;

namespace Helpers.Models
{

	public class TorrentItem
	{
		private XmlNode node;

		public TorrentItem()
		{
		}

		public TorrentItem(XmlNode node)
		{
			this.node = node;
			foreach ( XmlNode n in node.ChildNodes )
			{
            switch ( n.Name )
            {
               case "filename":
                  Filename = n.InnerText;
                  break;
               case "type":
                  Type = n.InnerText;
                  break;
               case "title":
                  Title = n.InnerText;
                  break;
					case "libdate":
						LibraryDate = DateTime.Parse( n.InnerText );
						break;
               default:
                  break;
            }
			}

		}

		public string Filename { get; set; }  //  this is the torent guid
		public string Title { get; set; }
		public string Type { get; set; }

		public DateTime LibraryDate { get; set; }
	}
}