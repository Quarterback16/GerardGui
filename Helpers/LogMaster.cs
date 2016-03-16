using Helpers.Interfaces;
using Helpers.Models;
using NLog;
using System.IO;
using System.Xml;

namespace Helpers
{
   public class LogMaster : XmlCache
   {
      public LogMaster(string logFileName) : base("Logs")
      {
         XmlDoc = new XmlDocument();
         XmlDoc.Load(logFileName);
         var listNode = XmlDoc.ChildNodes[2];  //  my convention is to always have a comment in ur xml file
         foreach (XmlNode node in listNode.ChildNodes)
            AddXmlLog(node);
      }

      private void AddXmlLog(XmlNode node)
      {
         AddLogItem(new LogItem(node));
      }

      private void AddLogItem(LogItem logItem)
      {
         PutItem(logItem);
      }

      public void PutItem(LogItem m)
      {
         var filespec = m.Filespec;
         if (!TheHT.ContainsKey(filespec))
         {
            TheHT.Add(filespec, m);
            IsDirty = true;
         }
      }

      #region Persistence

      /// <summary>
      ///   Converts the memory hash table to XML
      /// </summary>
      public void Dump2Xml()
      {
         if ((TheHT.Count > 0) && IsDirty)
         {
            var writer = new XmlTextWriter(string.Format("{0}", Filename), null);

            writer.WriteStartDocument();
            writer.WriteComment("Comments: " + Name);
            writer.WriteStartElement("logfile-list");

            var myEnumerator = TheHT.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
               var m = (LogItem)myEnumerator.Value;
               WriteLogNode(writer, m);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            Utility.Announce(string.Format("{0} created.", Filename));
         }
         else
            Utility.Announce(string.Format("No changes to {0}.", Filename));
      }

      private void WriteLogNode(XmlTextWriter writer, LogItem m)
      {
         writer.WriteStartElement("logfile-item");
         WriteElement(writer, "log-dir", m.LogDir);
         WriteElement(writer, "filespec", m.Filespec);
         WriteElement(writer, "datemailed", string.Format("{0:u}", m.MailDate));
         writer.WriteEndElement();
      }

      #endregion Persistence
   }
}
