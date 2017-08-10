using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class NFLPlayerTests
	{
		#region  cut Initialisation

		private NFLPlayer cut;

		[TestInitialize]
		public void TestInitialize()
		{
			cut = ClassUnderTest();
		}

		private static NFLPlayer ClassUnderTest()
		{
			return new NFLPlayer("MARIMA02");
		}

		#endregion

		[TestMethod]
		public void TestHealthRating()
		{
			var result = cut.HealthRating();
			Assert.AreEqual( expected: 0.1M, actual: result );
		}
	}
}
