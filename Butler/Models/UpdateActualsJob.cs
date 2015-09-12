using System;
using RosterLib.Interfaces;
using NLog;
using RosterLib;
using RosterLib.RosterGridReports;

namespace Butler.Models
{
   /// <summary>
   ///   Updates the PGMETRICS table
   /// </summary>
   public class UpdateActualsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public UpdateActualsJob( IKeepTheTime timeKeeper )
      {
         Name = "Update Actual Player Game Metrics";
         Console.WriteLine( "Constructing {0} ...", Name );
         Report = new MetricsUpdateReport();   //  do this
         TimeKeeper = timeKeeper;
         Logger = LogManager.GetCurrentClassLogger();
      }

      public override string DoJob()
      {
         Report.RenderAsHtml(); //  the old method that does the work
         return string.Format( "Rendered {0} to {1}", Report.Name, Report.OutputFilename() );
      }

      //  new business logic as to when to do the job
      public override bool IsTimeTodo( out string whyNot )
      {
         whyNot = string.Empty;
         if ( OnHold() ) whyNot = "Job is on hold";
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if ( !TimeKeeper.IsItRegularSeason() )
               whyNot = "Its not the Regular Season yet";
         }
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if ( !TimeKeeper.IsItWednesdayOrThursday( DateTime.Now ) )
               whyNot = "Its not Wednesday or Thursday";
         }
         return ( string.IsNullOrEmpty( whyNot ) );
      }
   }
}
