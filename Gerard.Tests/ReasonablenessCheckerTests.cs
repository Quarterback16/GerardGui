using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class ReasonablenessCheckerTests
	{
		#region  Sut Initialisation

		private ReasonablenessChecker sut;

		[TestInitialize]
		public void TestInitialize()
		{
			sut = SystemUnderTest();
		}

		private static ReasonablenessChecker SystemUnderTest()
		{
			return new ReasonablenessChecker();
		}

		#endregion

		[TestMethod]
		public void TestUnKnownKey_Defaults_To_Okay()
		{
			var result = sut.IsNotReasonable( "Total-???", 98.0M );
			Assert.IsFalse( result );
		}

		[TestMethod]
		public void TestTotalSacks()
		{
			var result = sut.IsNotReasonable( Constants.K_CHECK_TOTAL_SACKS, 98.0M );
			Assert.IsTrue( result );
		}

		[TestMethod]
		public void TestWeek12sacksForKC()
		{
			var g = new NFLGame( "2016:12-M" );
			var dbAllowed = g.GetTeamStats( 
				Constants.K_STATCODE_SACK, g.Season, g.Week, g.GameCode, g.AwayTeam );
			Assert.AreEqual( expected:5.0M, actual: dbAllowed );
		}

		[TestMethod]
		public void TestWeek12sacksForDB()
		{
			var g = new NFLGame( "2016:12-M" );
			var dbAllowed = g.GetTeamStats(
				Constants.K_STATCODE_SACK, g.Season, g.Week, g.GameCode, g.HomeTeam );
			Assert.AreEqual( expected: 6.0M, actual: dbAllowed );
		}

	}
}
