using RosterLib.Interfaces;
using System.Collections.Generic;
using RosterLib;

namespace Gerard.Tests
{
	public class FakeLoadRunners : ILoadRunners
	{
		public List<NFLPlayer> Load( string teamCode )
		{
			var playerList = new List<NFLPlayer>
			{
				new FakeNFLPlayer( "S", "RB", "Jonny the Starter" ),
				new FakeNFLPlayer( "B", "RB", "Buddy the Backup" )
			};
			return playerList;
		}
	}
}
