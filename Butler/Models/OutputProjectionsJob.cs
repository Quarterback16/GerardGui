using Butler.Interfaces;
using RosterLib;
using System;

namespace Butler.Models
{
   public class OutputProjectionsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public IHistorian Historian { get; set; }

      public OutputProjectionsJob(IHistorian historian)
      {
         Name = "Team Output Projections";
         Console.WriteLine("Constructing {0} ...", Name);
         Report = new ScoreTally();
         Historian = historian;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} job..............................................", Name );
         Report.Season = Utility.CurrentSeason();
         Report.RenderAsHtml(); //  the old method that does the work
         var finishedMessage = string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
         Logger.Info( "  {0}", finishedMessage );
         return finishedMessage;
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