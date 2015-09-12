using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
    public class Safety : BaseScore
    {
        public override string ScoreType { get; set; }

        public override string Name
        {
            get
            {
                return "Safety";
            }
        }
        public Safety()
        {
            ScoreType = "S";
        }

    }
}
