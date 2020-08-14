using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class GameProjectionsReportTests
	{
		[TestMethod]
		public void TestGameProjectionsReport()
		{
			var cut = new GameProjectionsReport(
				new FakeTimeKeeper(
					"2020",
					"00" ) );
			Assert.IsNotNull( cut );
			cut.RenderAsHtml();
		}

        [TestMethod]
        public void TestGameProjectionsReportStructure()
        {
            var cut = new GameProjectionsReport(
                new FakeTimeKeeper(
                    "2019",
                    "12"));
            Assert.IsNotNull(cut);
            cut.RenderAsHtml(structOnly: true);
        }


        [TestMethod]
		public void TestSingleGameProjectionsReport()
		{
			var cut = new NFLGame( "2017:01-I" );
			cut.WriteProjection();
		}
	}
}