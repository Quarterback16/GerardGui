using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
    public class FieldGoal : BaseScore
    {
        public override string ScoreType { get; set; }

        public override string Name
        {
            get
            {
                return "Field Goal";
            }
        }

        public FieldGoal()
        {
            ScoreType = "3";
        }
    }
}
