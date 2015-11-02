using System;
using NLog;
using RosterLib.Interfaces;
using RosterLib;

namespace Butler.Models
{
   public class YahooXmlFullJob : ReportJob
   {
      public bool FullSeason { get; set; }

      public YahooXmlFullJob(IKeepTheTime timekeeper)
      {
         Name = "Yahoo Xml job";
         Report = new YahooMasterGenerator(fullSeason: true);
         TimeKeeper = timekeeper;
         IsNflRelated = true;
         Logger = LogManager.GetCurrentClassLogger();
      }

      public override bool IsTimeTodo(out string whyNot)
      {
         whyNot = string.Empty;
         base.IsTimeTodo(out whyNot);
         if (string.IsNullOrEmpty(whyNot))
         {
            //  check if there is any new data
            whyNot = Report.CheckLastRunDate();
            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
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

      public override string DoJob()
      {
         if (Logger == null)
            Logger = LogManager.GetCurrentClassLogger();

         Logger.Info("Doing {0} job..............................................", Name);
         Report.RenderFullAsHtml(); //  the old method that does the work
         Report.Finish();
         var completionMsg = string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
         Logger.Info(completionMsg);
         return completionMsg;
      }
   }
}

