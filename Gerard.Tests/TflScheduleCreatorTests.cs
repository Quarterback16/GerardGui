using Butler.Events;
using Butler.Implementations;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Gerard.Tests
{
    [TestClass]
    public class TflScheduleCreatorTests
    {
        [TestMethod]
        public void Creator_CanInsertRecord()
        {
            var cut = new TflScheduleCreator();
            var result = cut.InsertGame(
                season: "2020",
                week: "01",
                gameNumber: "A",
                gameDate: new DateTime(2020, 9, 10, 20, 20, 0),
                gameHour: "8",
                awayTeamCode: "HT",
                homeTeamCode: "KC");
            Assert.AreEqual(
                "2020:01:A",
                result);
            Console.WriteLine(result);
        }

        [TestMethod]
        public void Creator_CanInsertSchedule()
        {
            var cut = new TflScheduleCreator();
            var eventStore = new ScheduleEventStore();
            var events = (List<ScheduleEvent>)eventStore
                .Get<ScheduleEvent>("schedule");

            var lastRound = 0;
            var gameNumber = 0;
            var totalGames = 0;
            foreach (var e in events)
            {
                var theGame = new Game(e);
                if (theGame.Round != lastRound)
                {
                    lastRound = theGame.Round;
                    gameNumber = 0;
                }
                gameNumber++;
                cut.ProcessGame(theGame, gameNumber);
                totalGames++;
            }
            Console.WriteLine($"{totalGames} processed");
        }
    }
}
