using NLog;
using RosterLib;
using RosterLib.Interfaces;
using RosterLib.RosterGridReports;
using System;

namespace Butler.Models
{
    public class LineupSummaryJob : Job
    {
        public int Week { get; set; }
        public RosterGridReport Report { get; set; }

        public LineupSummaryJob( IKeepTheTime timekeeper ) : base( timekeeper )
        {
            Name = "Lineup Summary";
            TimeKeeper = timekeeper;
            Logger = LogManager.GetCurrentClassLogger();
            Week = Int32.Parse( TimeKeeper.PreviousWeek() );
            Report = new LineupSummary( TimeKeeper, Week );
        }

        public override string DoJob()
        {
            Report.RenderAsHtml();
            Report.Finish();
            return $"Rendered {Report.Name} to {Report.OutputFilename()}";
        }

        public override bool IsTimeTodo( out string whyNot )
        {
            whyNot = string.Empty;
            if ( OnHold() ) whyNot = "Job is on hold";
            if ( string.IsNullOrEmpty( whyNot ) )
            {
                //  check if there is any new data
                whyNot = Report.CheckLastRunDate();
                if ( TimeKeeper.IsItPeakTime() )
                    whyNot = "Peak time - no noise please";
            }
            if ( !TimeKeeper.IsItWednesdayOrThursday( DateTime.Now ) )
                whyNot = "Only runs on Wednesday";
            if ( !string.IsNullOrEmpty( whyNot ) )
                Logger.Info( "Skipped {1}: {0}", whyNot, Name );
            return ( string.IsNullOrEmpty( whyNot ) );
        }

    }
}
