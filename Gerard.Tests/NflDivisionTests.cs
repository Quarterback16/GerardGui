using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class NflDivisionTests
	{
		private NFLDivision _sut;

		[TestInitialize]
		public void TestInitialize()
		{
		}

		[TestCleanup]
		public void TearDown()
		{
		}

		[TestMethod]
		public void TestQuickConstructor()
		{
			var sut = new NFLDivision( nameIn: "AFC - West", confIn: "A", codeIn:"H", seasonIn:"2017" );
			sut.DumpTeams();
			Assert.IsTrue( sut.TeamList.Count == 4 );  // Divisions all have 4 teams
		}

		[TestMethod]
		public void TestOldConstructor()
		{
			var sut = new NFLDivision( nameIn: "NFC - West", confIn: "N", codeIn: "D", seasonIn: "2017", catIn: "*" );
			sut.DumpTeams();
			Assert.IsTrue( sut.TeamList.Count == 4 );  // Divisions all have 4 teams
		}
	}
}
