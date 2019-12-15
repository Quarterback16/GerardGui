using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Butler.Implementations
{
    public class Record
    {
        public string Name { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public override string ToString()
        {
            return $"{Name}: ({Wins,2}-{Losses,2}) {Percent(),5}";
        }

        public string Percent()
        {
            if (TotalGames() == 0)
                return "0%";
            return $"{  Wins / (decimal)TotalGames() * 100M:####0}%";
        }

        public string OddsOut()
        {
            return $"{Odds():#0.0} to 1";
        }

        public decimal Odds()
        {
            if (TotalGames() == 0)
                return 0.0M;
            var percent = Wins / (decimal)TotalGames();
            if (percent == 0.0M)
                return percent;
            var odds = 1.0M / percent;
            odds -= 1.0M;
            return odds;
        }

        public decimal Dollars()
        {
            return Odds() + 1.0M;
        }

        private int TotalGames()
        {
            return Wins + Losses;
        }
    }
}
