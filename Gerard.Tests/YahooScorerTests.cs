using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class YahooScorerTests
	{
		#region  Sut Initialisation

		private YahooScorer sut;
		private NFLWeek testWeek;
		private NFLPlayer testPlayer;

		[TestInitialize]
		public void TestInitialize()
		{
			sut = SystemUnderTest();
		}

		private YahooScorer SystemUnderTest()
		{
			testWeek = new NFLWeek( "2016", "01" ); //  should use a fake week
			testPlayer = new NFLPlayer( "WILSRU01" );  //  Russell Wilson SS QB
			return new YahooScorer(testWeek);
		}

		#endregion


		[TestMethod]
		public void TestScoringAlgorith()
		{
			var result = sut.RatePlayer( testPlayer, testWeek);
			Assert.AreEqual( expected: 14.0M, actual: result );  //  history
		}
	}
}
