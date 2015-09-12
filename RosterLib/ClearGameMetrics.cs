using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosterLib
{
	public class ClearGameMetrics
	{
		public ClearGameMetrics( PlayerGameProjectionMessage input )
		{
			Process( input );
		}

		private void Process( PlayerGameProjectionMessage input )
		{
			input.Dao = new DbfPlayerGameMetricsDao();
			input.Dao.ClearGame( input.Game.GameKey() );
		}
	}
}
