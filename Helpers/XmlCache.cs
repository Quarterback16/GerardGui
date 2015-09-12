using System.Xml;
using System.Xml.XPath;
using System.Collections;

namespace Helpers
{
	public class XmlCache : ICache
	{
		protected XPathDocument epXmlDoc;
		protected XPathNavigator nav;

	   public XmlCache( string entityName )
		{
	      CacheHits = 0;
	      IsDirty = false;
	      CacheMisses = 0;
	      //RosterLib.Utility.Announce(string.Format("XmlCache.Init Constructing {0} master", entityName ) );

			Name = entityName;
			TheHT = new Hashtable();
		}

	   public int CacheHits { get; set; }

	   public int CacheMisses { get; set; }

	   public bool IsDirty { get; set; }

	   public XmlDocument XmlDoc { get; set; }

	   public Hashtable TheHT { get; set; }

	   public string Filename { get; set; }

	   public string Name { get; set; }

	   public string StatsMessage()
		{
			return string.Format( "{2} Cache hits {0} misses {1}", CacheHits, CacheMisses, Name );
		}

		public static void WriteElement( XmlTextWriter writer, string name, string text )
		{
			writer.WriteStartElement( name );
			writer.WriteString( text );
			writer.WriteEndElement();
		}

		public void DumpHt()
		{
			var myEnumerator = TheHT.GetEnumerator();
			var i = 0;
			Utility.Announce( "\t-INDEX-\t-KEY-\t-VALUE-" );
			while ( myEnumerator.MoveNext() )
				Utility.Announce( string.Format( "\t[{0}]:\t{1}\t{2}", i++, myEnumerator.Key, myEnumerator.Value ) );
		}
	}
}
