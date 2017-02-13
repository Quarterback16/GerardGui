﻿namespace RosterLib.ReportGenerators
{
   public class RookieReportGenerator
   {

      public PlayerLister Lister { get; set; }

      public RookieReportGenerator()
      {
         Lister = new PlayerLister();
      }

      public string GenerateRookieReport( 
         RookieConfig rpt, string fantasyLeague, string season )
      {
         Lister.StartersOnly = false;
         Lister.Clear();
         Lister.Collect( rpt.Category, rpt.Position, fantasyLeague, season );
         Lister.Folder = "Rookies";

         var fileOut = Lister.Render( 
            string.Format( "{1}-Rookies-{0}", rpt.Position, fantasyLeague ) );

         Lister.Clear();

         return fileOut;
      }
   }
}
