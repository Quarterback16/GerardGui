using System;
using NLog;
using RosterLib.Interfaces;
using RosterLib;

namespace Butler.Models
{
   public class YahooXmlJob : ReportJob
   {
      public bool FullSeason { get; set; }

      public YahooXmlJob( IKeepTheTime timekeeper )
      {
         Name = "Yahoo Xml job";
         Report = new YahooMasterGenerator(FullSeason, timekeeper);
         TimeKeeper = timekeeper;
         IsNflRelated = true;
         Logger = LogManager.GetCurrentClassLogger();
      }

      public override bool IsTimeTodo( out string whyNot )
      {
         whyNot = string.Empty;
         base.IsTimeTodo(out whyNot);
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return ( string.IsNullOrEmpty( whyNot ) );
      }

   }
}
