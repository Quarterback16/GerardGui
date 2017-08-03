using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
   public class RookiesJob : Job
   {
      public RosterGridReport Report { get; set; }

      public RookiesJob(IKeepTheTime timeKeeper)
         : base()
      {
         Name = nameof( Rookies );
         TimeKeeper = timeKeeper;
         Report = new Rookies(timeKeeper);
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         return Report.DoReport();
      }

      //  new business logic as to when to do the job
      public override bool IsTimeTodo(out string whyNot)
      {
         base.IsTimeTodo(out whyNot);
         if ( TimeKeeper.IsItPeakTime() )
            whyNot = "Peak time - no noise please";
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }
   }
}
