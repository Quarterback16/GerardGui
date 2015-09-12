using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
    public class PuntReturn : BaseScore
    {
        public override string ScoreType { get; set; }

        public override string Name
        {
            get
            {
                return "Punt Return";
            }
        }

        public PuntReturn()
        {
            ScoreType = "T";
        }
    }
}
