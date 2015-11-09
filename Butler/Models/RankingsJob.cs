using System;
using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class RankingsJob : Job
	{
		public RankingsJob( IKeepTheTime timekeeper )
      {
         Name = "Rankings Job";
         Console.WriteLine("Constructing {0} ...", Name);
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
      }

		public override string DoJob()
		{
			Logger.Info( "Doing {0} job..............................................", Name );
			var myRanker = new TeamRanker(TimeKeeper) {ForceReRank = true};
			myRanker.RankTeams( TimeKeeper.CurrentDateTime() );
			return string.Format( "Rendered {0} to {1}", Name, myRanker.FileOut );
		}
	}
}
