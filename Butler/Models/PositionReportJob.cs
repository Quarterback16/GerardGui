using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
   public class PositionReportJob : Job
   {
      public RosterGridReport Report { get; set; }

      public PositionReportJob( IKeepTheTime timekeeper )
      {
         Name = "Depth Charts";
         Report = new PositionReport( timekeeper );
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }
   }
}
