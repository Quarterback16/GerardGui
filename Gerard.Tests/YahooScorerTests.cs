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
			testWeek = new NFLWeek( "2017", "05" ); //  should use a fake week
			testPlayer = new NFLPlayer( "WATSDE02" );  //  Russell Wilson SS QB
			return new YahooScorer(testWeek);
		}

		#endregion


		[TestMethod]
		public void TestScoringAlgorith()
		{
			var result = sut.RatePlayer( testPlayer, testWeek, true);
			Assert.AreEqual( expected: 35.54M, actual: result );  //  history
		}
	}
}
