using Helpers.Interfaces;
using System.Configuration;

namespace Helpers
{
   public class ConfigReader : IConfigReader
   {
      public ConfigReader()
      {
      }

      public string GetSetting( string settingKey )
      {
         var setting = ConfigurationManager.AppSettings[ settingKey ];
         return setting;
      }
   }
}
