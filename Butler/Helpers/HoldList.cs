using Butler.Interfaces;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace Butler.Helpers
{
   public class HoldList : IHoldList
	{
		public List<string> Items { get; set; }

		public void LoadFromXml( string xmlFile )
		{
			//  reload every time so we can just fiddle the XML at any time
			Items = new List<string>();

         if (File.Exists(xmlFile))
         {
            using ( var r = new XmlTextReader( xmlFile ) )
            {
               while ( r.Read() )
               {
                  if ( r.NodeType != XmlNodeType.Element || r.Name != "hold" ) continue;
                  var item = r.ReadElementContentAsString();
                  Items.Add( item );
               }
               r.Close();
            }
         }
		}

		public bool Contains(string holdItem)
		{
			var contains = false;
			if ( Items != null )
			{
				contains = Items.Contains(holdItem);
			}
			return contains;
		}

	}
}
