using Butler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Butler
{
    public class TimeKeeper :IKeepTheTime
    {
        public bool IsItPreseason()
        {
            return true;   //TODO  :  implement
        }

        public DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
