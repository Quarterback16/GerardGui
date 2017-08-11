using RosterLib.Interfaces;

namespace RosterLib
{
	public class CommitteeAllocateYDrStrategy : IAllocateYDrStrategy
	{
		public void Allocate( RushUnit ru, int nYDr, PlayerGameMetricsCollection pgms )
		{
			//  45% 45%
			var projYDr = ( int ) ( 0.45M * nYDr );
			var pgm = pgms.GetPgmFor( ru.R1.PlayerCode );
			pgm.ProjYDr += ( int ) ( projYDr * ru.R1.HealthFactor() );
			pgms.Update( pgm );

			var projYDr2 = ( int ) ( 0.45M * nYDr );
			var pgm2 = pgms.GetPgmFor( ru.R2.PlayerCode );
			pgm2.ProjYDr += projYDr2;
			pgms.Update( pgm2 );

		}
	}
}
