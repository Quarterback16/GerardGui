using NLog;

namespace RosterLib
{
   public class YahooMasterGenerator : RosterGridReport
   {

      public YahooMaster YahooMaster { get; set; }
      public YahooMasterGenerator()
      {
         Name = "Yahoo Master Generator";
         YahooMaster = new YahooMaster("Yahoo", "YahooOutput.xml");
         Logger = LogManager.GetCurrentClassLogger();
      }

      public override void RenderAsHtml()
      {
         YahooMaster.Calculate(Utility.CurrentSeason(), Utility.PreviousWeekAsString());
         YahooMaster.Dump2Xml();
      }

      public override string OutputFilename()
      {
         return YahooMaster.Filename;
      }
   }
}
