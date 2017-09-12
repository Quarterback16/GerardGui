using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class NflSeasonTests
	{
		[TestMethod]
		public void NflSeason_Constructs_ok() 
		{
			var season = new NflSeason( "2017", teamsOnly: true );
			season.DumpTeams();
			foreach ( var team in season.TeamList )
			{
				System.Console.WriteLine( team.NameOut() ); 
			}

			Assert.IsNotNull( season );
		}
	}
}
