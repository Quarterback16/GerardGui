using Butler.Interfaces;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
   public class OutputProjectionsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public IHistorian Historian { get; set; }

      public OutputProjectionsJob(
          IKeepTheTime timekeeper,
          IHistorian historian) : base( timekeeper )
		{
         Name = "Team Output Projections";
         Report = new ScoreTally( timekeeper );
         Historian = historian;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         return Report.DoReport();
      }

      public override bool IsTimeTodo(out string whyNot)
      {
         whyNot = string.Empty;
         base.IsTimeTodo(out whyNot);
         if (string.IsNullOrEmpty(whyNot))
         {
#if ! DEBUG

               //  check if there is any new data
               whyNot = Report.CheckLastRunDate();

#endif
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }
   }
}