using NLog;
using RosterLib;
using RosterLib.Interfaces;
using System;

namespace Butler.Models
{
   public class RankingsJob : Job
	{
      public TeamRanker TeamRanker { get; set; }

      public DateTime RankDate { get; set; }

      public RankingsJob( IKeepTheTime timekeeper, bool force = false )
      {
         Name = "Rankings Job";
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
         TeamRanker = new TeamRanker( TimeKeeper ) { ForceReRank = force };
         RankDate = TimeKeeper.CurrentDateTime();
         IsNflRelated = true;
      }

      public override string DoJob()
		{
			TeamRanker.RankTeams( RankDate );
			return string.Format( "Rendered {0} to {1}", Name, TeamRanker.FileOut );
		}

      public override bool IsTimeTodo( out string whyNot )
      {
         base.IsTimeTodo( out whyNot );

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
