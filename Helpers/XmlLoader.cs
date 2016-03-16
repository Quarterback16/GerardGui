using Helpers.Interfaces;
using System.Collections.Generic;
using System.Xml;

namespace Helpers
{
   public class XmlLoader : ILoadXml
   {
      public List<string> LoadFromXml(string xmlFileName, string nodeName, string attributeName = "")
      {
         var items = new List<string>();
         if (string.IsNullOrEmpty(xmlFileName) || string.IsNullOrEmpty(nodeName))
            return items;

         var r = new XmlTextReader(xmlFileName);
         while (r.Read())
         {
            string item = string.Empty;
            if (r.NodeType != XmlNodeType.Element || r.Name != nodeName) continue;
            if (string.IsNullOrEmpty(attributeName))
            {
               item = r.ReadElementContentAsString();
            }
            else
            {
               item = r.GetAttribute(attributeName);
            }
            if (string.IsNullOrEmpty(item)) continue;
            items.Add(item);
         }
         r.Close();

         return items;
      }
   }
}
