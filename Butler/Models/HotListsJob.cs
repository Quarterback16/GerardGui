﻿using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
    public class HotListsJob : Job
    {
        public RosterGridReport Report { get; set; }

        public HotListsJob( IKeepTheTime timekeeper )
        {
            Name = "Hot Lists";
            Report = new HotListReporter( timekeeper );
            TimeKeeper = timekeeper;
            Logger = NLog.LogManager.GetCurrentClassLogger();
            IsNflRelated = true;
        }

        public override string DoJob()
        {
            return Report.DoReport();
        }

        //  new business logic as to when to do the job
        public override bool IsTimeTodo( out string whyNot )
        {
            whyNot = string.Empty;
            base.IsTimeTodo( out whyNot );

            if ( string.IsNullOrEmpty( whyNot ) )
                //  check if there is any new data
                whyNot = Report.CheckLastRunDate();
            if ( string.IsNullOrEmpty( whyNot ) )
            {
                if ( TimeKeeper.IsItPeakTime() )
                    whyNot = "Peak time - no noise please";
            }
            if ( TimeKeeper.IsItTuesday() )
                whyNot = "Not on Tuesdays";

            if ( !string.IsNullOrEmpty( whyNot ) )
                Logger.Info( "Skipped {1}: {0}", whyNot, Name );
            return ( string.IsNullOrEmpty( whyNot ) );
        }
    }
}