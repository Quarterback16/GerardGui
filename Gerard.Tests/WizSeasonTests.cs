using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
    [TestClass]
    public class WizSeasonTests
    {
        #region  Sut Initialisation

        private WizSeason sut;

        [TestInitialize]
        public void TestInitialize()
        {
            sut = SystemUnderTest();
        }

        private static WizSeason SystemUnderTest()
        {
            return new WizSeason();
        }

        #endregion

        [TestMethod]
        public void TestCurrentSeason()
        {
            sut.LoadSeason(
                seasonIn: "2019",
                startWeekIn: "01",
                endWeekIn: "15");

            sut.OutputPhase();
        }
    }
}
