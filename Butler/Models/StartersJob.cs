using RosterLib.Interfaces;
using RosterLib;

namespace Butler.Models
{
   public class StartersJob : Job
   {
      public RosterGridReport Report { get; set; }

      public StartersJob(IKeepTheTime timeKeeper) : base()
      {
         Name = "Starters";
         TimeKeeper = timeKeeper;
         Report = new Starters(doCsv:false);  //  separate job now for the CSV
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         return Report.DoReport();
      }

      public override bool IsTimeTodo(out string whyNot)
      {
         base.IsTimeTodo(out whyNot);

         if (string.IsNullOrEmpty(whyNot))
         {
#if ! DEBUG
            #endif
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }

   }
}