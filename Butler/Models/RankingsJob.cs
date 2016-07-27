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
      }

      public override string DoJob()
		{
			TeamRanker.RankTeams( RankDate );
			return string.Format( "Rendered {0} to {1}", Name, TeamRanker.FileOut );
		}
	}
}
