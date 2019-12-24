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

        [TestMethod]
        public void TestDak()
        {
            var p = new NFLPlayer("PRESDA01");
            var result = sut.Calculate(
                p: p,
                statType: "P",
                quantity: 275);
            Assert.IsNotNull(result);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result
                }   Odds: {
                result.OddsOut()
                }   $:{result.Dollars():#0.00}");
        }

        [TestMethod]
        public void TestJoshAllen()
        {
            var p = new NFLPlayer("ALLEJO02");
            var result = sut.Calculate(
                p: p,
                statType: "P",
                quantity: 200);
            Assert.IsNotNull(result);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result
                }   Odds: {
                result.OddsOut()
                }   $:{result.Dollars():#0.00}");

            Console.WriteLine();
            var result2 = sut.Calculate(
                p: p,
                statType: "P",
                quantity: 200,
                GameScenario.ShortDog);
            Assert.IsNotNull(result2);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result2
                }   Odds: {
                result2.OddsOut()
                }   $:{result2.Dollars():#0.00}");
        }


        [TestMethod]
        public void TestGoff()
        {
            var p = new NFLPlayer("GOFFJA01");
            var result = sut.Calculate(
                p: p,
                statType: "P",
                quantity: 300);
            Assert.IsNotNull(result);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result
                }   Odds: {
                result.OddsOut()
                }   $:{result.Dollars():#0.00}");
        }

        [TestMethod]
        public void TestWentz()
        {
            var p = new NFLPlayer("WENTCA01");
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

        [TestMethod]
        public void TestJackson()
        {
            var p = new NFLPlayer("JACKLA02");
            var result = sut.Calculate(
                p: p,
                statType: "P",
                quantity: 200);
            Assert.IsNotNull(result);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result
                }   Odds: {
                result.OddsOut()
                }   $:{result.Dollars():#0.00}");
        }

        [TestMethod]
        public void TestBrees()
        {
            var p = new NFLPlayer("BREEDR01");
            var result = sut.Calculate(
                p: p,
                statType: "P",
                quantity: 300);
            Assert.IsNotNull(result);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result
                }   Odds: {
                result.OddsOut()
                }   $:{result.Dollars():#0.00}");

            Console.WriteLine();
            var result2 = sut.Calculate(
                p: p,
                statType: "P",
                quantity: 300,
                GameScenario.LongFavourite);
            Assert.IsNotNull(result2);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result2
                }   Odds: {
                result2.OddsOut()
                }   $:{result2.Dollars():#0.00}");

            Console.WriteLine();
            var result3 = sut.Calculate(
                p: p,
                statType: "P",
                quantity: 300,
                GameScenario.ShortFavourite);
            Assert.IsNotNull(result3);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result3
                }   Odds: {
                result3.OddsOut()
                }   $:{result3.Dollars():#0.00}");
        }

        [TestMethod]
        public void TestJimmyG()
        {
            var propYds = 250;
            var p = new NFLPlayer("GAROJI01");
            var result = sut.Calculate(
                p: p,
                statType: "P",
                quantity: propYds);
            Assert.IsNotNull(result);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result
                }   Odds: {
                result.OddsOut()
                }   $:{result.Dollars():#0.00}");

            Console.WriteLine();
            var result2 = sut.Calculate(
                p: p,
                statType: "P",
                quantity: propYds,
                GameScenario.LongFavourite);
            Assert.IsNotNull(result2);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result2
                }   Odds: {
                result2.OddsOut()
                }   $:{result2.Dollars():#0.00}");

            Console.WriteLine();
            var result3 = sut.Calculate(
                p: p,
                statType: "P",
                quantity: propYds,
                GameScenario.ShortFavourite);
            Assert.IsNotNull(result3);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result3
                }   Odds: {
                result3.OddsOut()
                }   $:{result3.Dollars():#0.00}");
        }


        [TestMethod]
        public void TestOdds()
        {
            var result = new Record(2,2);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result
                }   Odds: {
                result.OddsOut()
                }   $:{result.Dollars():#0.00}");
            Assert.AreEqual(2.00M, result.Dollars());
        }

        [TestMethod]
        public void TestOddsForPercent()
        {
            var result = new Record(74,26);
            Console.WriteLine();
            Console.WriteLine($@"Record: {
                result
                }   Odds: {
                result.OddsOut()
                }   $:{result.Dollars():#0.00}");
        }
    }
}
