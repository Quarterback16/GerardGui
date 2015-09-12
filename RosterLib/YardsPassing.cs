using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosterLib
{
    public class YardsPassing : BaseStat
    {

        #region IStat Members

        public override string Name
        {
            get
            {
                return "Yards Passing";
            }
        }

        public override bool IsReasonable()
        {

            if ( (Quantity >= 600) || (Quantity < -20) )
                return false;
            else
                return true;

        }

        #endregion
    }
}
