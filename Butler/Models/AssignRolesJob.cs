using System;
using RosterLib.Interfaces;
using NLog;
using RosterLib;

namespace Butler.Models
{
   public class AssignRolesJob : Job
   {
      public RosterGridReport Report { get; set; }

      public AssignRolesJob( IKeepTheTime timeKeeper )
      {
         Name = "Assign Roles";
         Report = new RoleAssignmentReport(timeKeeper);
         TimeKeeper = timeKeeper;
         Logger = LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
      }

      public override string DoJob()
      {
         return Report.DoReport();
      }

      //  new business logic as to when to do the job
      public override bool IsTimeTodo( out string whyNot )
      {
         //   Only want to do once so as not to overwrite manual role setting

         base.IsTimeTodo(out whyNot);
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if ( !TimeKeeper.IsItRegularSeason() )
               whyNot = "Its not the Regular Season yet";
         }
         if ( string.IsNullOrEmpty( whyNot ) )
         {
            if ( !TimeKeeper.IsItWednesday( DateTime.Now ) )
               whyNot = "Its not Wednesday";
         }
         if (string.IsNullOrEmpty(whyNot))
         {
            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
         }
         if (!string.IsNullOrEmpty( whyNot ))
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return ( string.IsNullOrEmpty( whyNot ) );
      }
   }
}