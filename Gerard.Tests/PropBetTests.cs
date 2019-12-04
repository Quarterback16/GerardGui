using Butler.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
    [TestClass]
    public class PropBetTests
    {
        #region  Sut Initialisation

        private PropBet sut;

        [TestInitialize]
        public void TestInitialize()
        {
            sut = SystemUnderTest();
        }

        private static PropBet SystemUnderTest()
        {
            return new PropBet();
        }

        #endregion

        [TestMethod]
        public void TestRusselWilson()
        {
            var p = new NFLPlayer("WILSRU01");
            var result = sut.Calculate(
                p: p,
                statType: "P",
                quantity: 250);
            Assert.IsNotNull(result);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result
                }   Odds: {
                result.OddsOut()
                }   $:{result.Dollars():#0.00}");
        }
    }
}
