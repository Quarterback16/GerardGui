using RosterLib.Interfaces;
using RosterLib;
using System;
using Butler.Implementations;

namespace Butler.Models
{
   public class GameProjectionReportsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public GameProjectionReportsJob( IKeepTheTime timeKeeper )
      {
         Name = "Game Projection Reports";
         Report = new GameProjectionsReport( timeKeeper );
         TimeKeeper = timeKeeper;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         return Report.DoReport();
      }

      public override bool IsTimeTodo( out string whyNot )
      {
         base.IsTimeTodo( out whyNot );

         if ( string.IsNullOrEmpty( whyNot ) )
         {
            //  check if there is any new data
            whyNot = Report.CheckLastRunDate();
         }
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if ( TimeKeeper.IsItPeakTime() )
               whyNot = string.Format( "{0:t} is peak time", DateTime.Now.TimeOfDay );
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return ( string.IsNullOrEmpty( whyNot ) );
      }
   }
}
