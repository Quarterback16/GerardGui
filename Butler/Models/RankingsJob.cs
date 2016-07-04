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
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();
      }

		public override string DoJob()
		{
			var myRanker = new TeamRanker(TimeKeeper) {ForceReRank = true};
			myRanker.RankTeams( TimeKeeper.CurrentDateTime() );
			return string.Format( "Rendered {0} to {1}", Name, myRanker.FileOut );
		}
	}
}
