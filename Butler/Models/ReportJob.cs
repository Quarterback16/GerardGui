using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
   public class ReportJob : Job
   {
      public RosterGridReport Report { get; set; }

	   public bool CheckLastRun { get; set; }

      public ReportJob()
      {
			CheckLastRun = true;
		}

      public ReportJob( IKeepTheTime timekeeper )
      {
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
	      CheckLastRun = true;
      }

      public ReportJob(
          IKeepTheTime timekeeper,
          RosterGridReport report )
      {
         Report = report;
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
			CheckLastRun = true;
		}

      public override bool IsTimeTodo( out string whyNot )
      {
	      base.IsTimeTodo(out whyNot);
         if (!string.IsNullOrEmpty(whyNot)) return (string.IsNullOrEmpty(whyNot));

	      if (CheckLastRun)
	      {
		      //  check if there is any new data
		      whyNot = Report.CheckLastRunDate();
				if ( !string.IsNullOrEmpty( whyNot ) ) return ( string.IsNullOrEmpty( whyNot ) );
	      }

	      if (TimeKeeper.IsItPeakTime())
            whyNot = "Peak time - no noise please";
         return ( string.IsNullOrEmpty( whyNot ) );
      }

      public override string DoJob()
      {
         if ( Logger == null )
            Logger = LogManager.GetCurrentClassLogger();

         return Report.DoReport();
      }

   }
}
