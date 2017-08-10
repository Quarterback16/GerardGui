using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class DbfPlayerGameMetricsDaoTests
	{
		#region  cut Initialisation

		private DbfPlayerGameMetricsDao cut;

		[TestInitialize]
		public void TestInitialize()
		{
			cut = ClassUnderTest();
		}

		private static DbfPlayerGameMetricsDao ClassUnderTest()
		{
			return new DbfPlayerGameMetricsDao();
		}

		#endregion

		[TestMethod]
		public void TestMarcusMariota2017()
		{
			var totPts = 0.0M;
			var p = new NFLPlayer( "MARIMA02" );
			var pgms = cut.GetSeason( "2017", p.PlayerCode );
			foreach ( PlayerGameMetrics pgm in pgms )
			{
				p.Points = pgm.CalculateProjectedFantasyPoints( p );
				totPts += p.Points;
			}
			Assert.AreEqual( expected: 226, actual: totPts );
		}
	}
}
