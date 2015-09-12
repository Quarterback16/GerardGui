using RosterLib.Interfaces;
using RosterLib;
using System;

namespace Butler.Models
{
   public class StartersJob : Job
   {
      public RosterGridReport Report { get; set; }

      public StartersJob(IKeepTheTime timeKeeper) : base()
      {
         Name = "Starters";
         Console.WriteLine("Constructing {0} ...", Name);
         TimeKeeper = timeKeeper;
         Report = new Starters(doCsv:false);  //  separate job now for the CSV
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} job..............................................", Name );
         Report.RenderAsHtml(); //  the old method that does the work
         Report.Finish();
         return string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
      }

      //  new business logic as to when to do the job
      public override bool IsTimeTodo(out string whyNot)
      {
         base.IsTimeTodo(out whyNot);

         if (string.IsNullOrEmpty(whyNot))
         {
#if ! DEBUG
            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
            if (string.IsNullOrEmpty(whyNot))
            {
               //  check if there is any new data
               whyNot = Report.CheckLastRunDate();
            }
         
#endif
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }

   }
}