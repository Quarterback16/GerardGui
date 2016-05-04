using RosterLib.Interfaces;
using RosterLib;
using System;

namespace Butler.Models
{
   public class RecordActualMetricsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public RecordActualMetricsJob(IKeepTheTime timeKeeper)
      {
         Name = "Record Actual Metrics";
         Console.WriteLine("Constructing {0} ...", Name);
         var season = timeKeeper.CurrentSeason( DateTime.Now );
         var week = string.Format( "{0:00}", timeKeeper.CurrentWeek(DateTime.Now) );
         Report = new RecordOfActualMetricsReport( season, week );
         TimeKeeper = timeKeeper;
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
            if (!TimeKeeper.IsItRegularSeason())
               whyNot = "Its not the Regular Season";
         }
         if (string.IsNullOrEmpty(whyNot))
         {
            //  check if there is any new data
            whyNot = Report.CheckLastRunDate();
            if (TimeKeeper.IsItPeakTime())
            {
               whyNot = "Peak time - no noise please";
            }
         }
         if (string.IsNullOrEmpty(whyNot))
         {
            if (!TimeKeeper.IsItWednesdayOrThursday(DateTime.Now))
               whyNot = "Its not Wednesday or Thursday";
         }
         if (!string.IsNullOrEmpty(whyNot))
            Logger.Info("Skipped {1}: {0}", whyNot, Name);

         return (string.IsNullOrEmpty(whyNot));
      }

   }
}
