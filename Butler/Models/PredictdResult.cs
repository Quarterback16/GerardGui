using Butler.Helpers;
using System;


namespace Butler.Models
{
    public class PredictedResult
    {
        public Game Game { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public bool HomeWin { get; set; }
        public bool AwayWin { get; set; }

        public PredictedResult(Game game)
        {
            Game = game;
        }

        public string Tip()
        {
            return $@"{
                GamePredictionOut(Game)
                } : Tip is {
                TipOut()
                } {
                MarginOut()
                }  {
                Schedina()}";
        }

        private string MarginOut()
        {
            return $@"({
                StringUtils.PadLeft(
                    3,
                    Math.Abs(HomeScore - AwayScore).ToString())
                })";
        }

        private string GamePredictionOut(Game game)
        {
            return $@"{
                Game.GameDate.ToString("yyyy-MM-dd HH:mm")
                } {
                StringUtils.StringOfSize(4, game.AwayTeam)
                } {
                StringUtils.PadLeft(3, AwayScore.ToString())
                } @ {
                StringUtils.StringOfSize(4, game.HomeTeam)
                } {
                StringUtils.PadLeft(3, HomeScore.ToString())
                }";
        }

        private string Schedina()
        {
            if (IsHomeWin())
                return "1";
            if (IsAwayWin())
                return "2";
            return "X";
        }

        private bool IsAwayWin()
        {
            return AwayScore > HomeScore;
        }

        private bool IsHomeWin()
        {
            return HomeScore > AwayScore;
        }

        private string TipOut()
        {
            if (IsHomeWin())
                return StringUtils.StringOfSize(4, Game.HomeTeam);
            else if (IsAwayWin())
                return StringUtils.StringOfSize(4, Game.AwayTeam);
            return "DRAW";
        }

        public string Result()
        {
            if (HomeScore > AwayScore)
            {
                HomeWin = true;
                AwayWin = false;
            }
            else if (AwayScore > HomeScore)
            {
                HomeWin = false;
                AwayWin = true;
            }
            else
            {
                HomeWin = false;
                AwayWin = false;
            }
            return $"{AwayScore} - {HomeScore}";
        }

        public override string ToString()
        {
            return $"{Result()} tip: {TipOut()}";
        }
    }
}
