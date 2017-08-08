using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class StatMasterTests
	{
		#region  Sut Initialisation

		private StatMaster sut;

		[TestInitialize]
		public void TestInitialize()
		{
			sut = SystemUnderTest();
		}

		private static StatMaster SystemUnderTest()
		{
			return new StatMaster( "Stats", "stats.xml" );
		}

		#endregion

		[TestMethod]
		public void TestGettingStats()
		{
			var result = sut.GetStat( "2016", "14", "WR", "YDp" );
			Assert.IsNotNull( result.Opponent );
		}
	}
}
