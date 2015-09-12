using RosterLib.Interfaces;
using RosterLib;
using System;

namespace Butler.Models
{
   public class HotListsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public HotListsJob()
      {
         Name = "Hot Lists";
         Console.WriteLine("Constructing {0} ...", Name);
         Report = new HotListReporter();
         TimeKeeper = new TimeKeeper();
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} ..............................................", Name );
         Report.RenderAsHtml(); //  the old method that does the work
         Report.Finish();
         var finishMessage = string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
         Logger.Info( "  {0}", finishMessage );
         return finishMessage;
      }

      //  new business logic as to when to do the job
      public override bool IsTimeTodo(out string whyNot)
      {
         base.IsTimeTodo(out whyNot);
         if (string.IsNullOrEmpty( whyNot ))
            //  check if there is any new data
            whyNot = Report.CheckLastRunDate();
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }
   }
}