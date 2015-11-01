using NLog;

namespace RosterLib
{
   public class YahooMasterGenerator : RosterGridReport
   {

      public YahooMaster YahooMaster { get; set; }

      public bool FullSeason { get; set; }
      public YahooMasterGenerator( bool fullSeason )
      {
         Name = "Yahoo Master Generator";
         YahooMaster = new YahooMaster("Yahoo", "YahooOutput.xml");
         Logger = LogManager.GetCurrentClassLogger();
         FullSeason = fullSeason;
      }

      public override void RenderAsHtml()
      {
         if ( FullSeason )
            YahooMaster.Calculate(Utility.CurrentSeason() );
         else
            YahooMaster.Calculate(Utility.CurrentSeason(), Utility.PreviousWeekAsString());
         YahooMaster.Dump2Xml();
      }

      public override string OutputFilename()
      {
         return YahooMaster.Filename;
      }
   }
}
