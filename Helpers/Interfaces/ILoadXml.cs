using System.Collections.Generic;
namespace Helpers.Interfaces
{
   public interface ILoadXml
   {
      List<string> LoadFromXml(string xmlFileName, string nodeName, string attributeName = "");
   }
}
