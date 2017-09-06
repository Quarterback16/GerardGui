using RosterLib;
using System.Collections.Generic;

namespace Butler.Interfaces
{
	public interface ISuggestLineups
	{
		List<NFLPlayer> SuggestedLineup( string leagueCode, string teamCode );
	}
}
