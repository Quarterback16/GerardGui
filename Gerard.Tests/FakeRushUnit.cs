using RosterLib;
using System.Collections.Generic;

namespace Gerard.Tests
{
	public class FakeRushUnit : RushUnit
	{
		public FakeRushUnit()
		{
			Runners = new List<NFLPlayer>
			{
				new FakeNFLPlayer( "S", "RB,SH,3D", "Mike do it all" )
			};
			Runners.Add( new FakeNFLPlayer( "B", "RB", "Sam backup RB" ) );
			SetSpecialRoles();
		}
	}
}