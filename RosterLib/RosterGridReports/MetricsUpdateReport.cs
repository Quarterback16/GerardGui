namespace RosterLib.RosterGridReports
{
   public class MetricsUpdateReport : RosterGridReport
   {
      public int Week { get; set; }

      public MetricsUpdateReport()
      {
         Name = "Metrics Update Report";
         Season = SeasonBoss.CurrentSeason();
         Week = Utility.CurrWeek;
      }

      public override string OutputFilename()
      {
         return string.Format( "{0}{1}/{2}.htm", Utility.OutputDirectory(), Season, Name );
      }

      public override void RenderAsHtml()
      {
         //  process and add lines to a pre report
         string body = "build report here";
         OutputReport( body );
         Finish();
      }

      private void OutputReport( string body )
      {
         var PreReport = new SimplePreReport
         {
            ReportType = Name,
            Folder = "Metrics",
            Season = Season,
            InstanceName = string.Format( "MetricsUpdates-{0:0#}", Week ),
            Body = body
         };
         PreReport.RenderHtml();
         FileOut = PreReport.FileOut;
      }
   }
}