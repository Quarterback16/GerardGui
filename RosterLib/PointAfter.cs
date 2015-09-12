using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
    public class PointAfter : BaseScore
    {
        public override string ScoreType { get; set; }

        public override string Name
        {
            get
            {
                return "PAT";
            }
        }
        public PointAfter()
        {
            ScoreType = "1";
        }
    }
}
