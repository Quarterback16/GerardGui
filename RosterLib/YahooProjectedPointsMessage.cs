using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{
	public class YahooProjectedPointsMessage
	{
		public NFLPlayer Player { get; set; }
		public NFLGame Game { get; set; }
		public PlayerGameMetrics PlayerGameMetrics { get; set; }
	}
}
