using NLog;
using RosterLib;
using System;
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
         Console.WriteLine("Constructing {0} ...", Name);
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
	      CheckLastRun = true;
      }

      public ReportJob( IKeepTheTime timekeeper, RosterGridReport report )
      {
         Console.WriteLine( "Constructing {0} ...", Name );
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

         Logger.Info( "Doing {0} job..............................................", Name );
         Report.RenderAsHtml(); //  the old method that does the work
         Report.Finish();
         var completionMsg = string.Format( "Rendered {0} to {1}", Report.Name, Report.OutputFilename() );
         Logger.Info( completionMsg );
         return completionMsg;
      }

   }
}
