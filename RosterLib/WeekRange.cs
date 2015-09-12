using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFLLib;

namespace RosterLib
{


	public class WeekRange
	{
		public NFLWeek startWeek{ get; set; }
		public NFLWeek endWeek{ get; set; }

		public bool Contains( NFLGame game )
		{
			//TODO:  implement
			return true;
		}
	}
}
