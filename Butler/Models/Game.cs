using Butler.Events;
using Butler.Helpers;
using System;

namespace Butler.Models
{
    public class Game
    {
        public string League { get; set; }
        public int Round { get; set; }
        public DateTime GameDate { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public PredictedResult PredictedResult { get; set; }

        public Game(ScheduleEvent e)
        {
            League = e.LeagueCode;
            Round = e.Round;
            GameDate = e.GameDate;
            HomeTeam = e.HomeTeam;
            AwayTeam = e.AwayTeam;
        }

        public Game(ResultEvent e)
        {
            League = e.LeagueCode;
            Round = e.Round;
            GameDate = e.GameDate;
            HomeTeam = e.HomeTeam;
            AwayTeam = e.AwayTeam;
            HomeScore = e.HomeScore;
            AwayScore = e.AwayScore;
        }

        public string Result()
        {
            return $"{AwayScore} - {HomeScore}";
        }

        public override string ToString()
        {
            return $@"{
                League
                } Round {
                Round
                } : {
                GameDate.ToString("yyyy-MM-dd HH:mm")
                } {
                StringUtils.StringOfSize(4, AwayTeam)
                } {AwayScore} @ {
                StringUtils.StringOfSize(4, HomeTeam)
                } {HomeScore}";
        }

        internal bool WinFor(string teamCode)
        {
            if (HomeTeam.Equals(teamCode))
            {
                if (HomeScore > AwayScore)
                    return true;
            }
            if (AwayTeam.Equals(teamCode))
                if (AwayScore > HomeScore)
                    return true;
            return false;
        }

        internal bool LossFor(string teamCode)
        {
            if (HomeTeam.Equals(teamCode))
            {
                if (AwayScore > HomeScore)
                    return true;
            }
            if (AwayTeam.Equals(teamCode))
                if (HomeScore > AwayScore)
                    return true;
            return false;
        }
    }
}
