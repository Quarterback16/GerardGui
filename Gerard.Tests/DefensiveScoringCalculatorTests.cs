using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
    [TestClass]
    public class DefensiveScoringCalculatorTests
    {
        private DefensiveScoringCalculator _sut;

        [TestInitialize]
        public void Setup()
        {
            var week = new NFLWeek(2018, 15);
            _sut = new DefensiveScoringCalculator(
                startWeek: week,
                offset: 0);
        }

        [TestMethod]
        public void Calculator_ForAf201815_Calculates18()
        {
            var team = new NflTeam("AF");
            var game = new NFLGame("AF","2018","15");
            _sut.Calculate(team, game);
            System.Console.WriteLine(team.FantasyPoints);
            Assert.AreEqual(18.0M, team.FantasyPoints);
        }

        [TestMethod]
        public void Calculator_OpponentForAf201815_Calculates18()
        {
            var team = new NflTeam("AF");
            var game = new NFLGame("AF", "2018", "15");
            var opp = game.OpponentTeam("AF");
            System.Console.WriteLine($"Opp:{opp}");
            _sut.Calculate(opp, game);
            Assert.AreEqual(-2.0M, opp.FantasyPoints);
        }
    }
}
