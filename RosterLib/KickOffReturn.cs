using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
    public class KickOffReturn : BaseScore
    {
        public override string ScoreType { get; set; }

        public override string Name
        {
            get
            {
                return "Kickoff Return";
            }
        }

        public KickOffReturn()
        {
            ScoreType = "K";
        }
    }
}
