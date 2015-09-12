namespace RosterLib
{
   public class PlayerCareerReport : RosterGridReport
   {
      public NFLRosterReport RosterReport { get; set; }

      public PlayerCareerReport( string season )
      {
         Season = season;
      }

      public override void RenderAsHtml()
      {
         Name = "Career Reports";
         RosterReport = new NFLRosterReport( Season );
         RosterReport.LoadAfc();
         RosterReport.PlayerReports();
      }

      public override string OutputFilename()
      {
         var fileName = string.Format( "{0}\\Players\\Errors.htm", Utility.OutputDirectory(), Season );
         return fileName;
      }
   }
}