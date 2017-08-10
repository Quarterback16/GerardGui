using RosterLib;
using RosterLib.Interfaces;
using System.Collections.Generic;

namespace Gerard.Tests
{
	//  Fakeloadrunner creates Rushing units based on a dummy teamCode
	public class FakeLoadRunners : ILoadRunners
	{
		public List<NFLPlayer> Load( string teamCode )
		{
			if ( teamCode.Equals( "NE" ) )
			{
				// standard 
				var playerList = new List<NFLPlayer>
				{
					new FakeNFLPlayer( "JS01", "S", "RB", "Jonny the Starter" ),
					new FakeNFLPlayer( "BB01", "B", "RB", "Buddy the Backup" )
				};
				return playerList;
			}
			else if ( teamCode.Equals( "AF" ) )
			{
				// standard
				var playerList = new List<NFLPlayer>
				{
					new FakeNFLPlayer( "VV01", "S", "RB", "Vick the Vet", "3" ),
					new FakeNFLPlayer( "BB02", "B", "RB", "Bro the Backup" ),
					new FakeNFLPlayer( "VU01", "R", "RB,SH", "Vulture the Goalline specialist" ),
				};
				return playerList;
			}
			else if ( teamCode.Equals( "BB" ) )
			{
				//  Ace
				var playerList = new List<NFLPlayer>
				{
					new FakeNFLPlayer( "BM01", "S", "RB,SH", "Beast Mode", "1" ),
					new FakeNFLPlayer( "VU01", "R", "RB,3D", "Trippy the 3rd down specialist" ),
				};
				return playerList;
			}
			else if ( teamCode.Equals( "BR" ) )
			{
				//  Committe
				var playerList = new List<NFLPlayer>
				{
					new FakeNFLPlayer( "CM01", "S", "RB,SH", "Committee 1" ),
					new FakeNFLPlayer( "CM02", "S", "RB,3D", "Committee 2" ),
				};
				return playerList;
			}
			else
			{
				return new List<NFLPlayer>();
			}
		}
	}
}