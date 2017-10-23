using System;

namespace RosterLib
{
	public class Pickup
	{
		public string Name { get; set; }

		public string Opp { get; set; }

		public decimal ProjPts { get; set; }

		public string ActualPts { get; set; }

		public string CategoryCode { get; set; }

		public string Category()
		{
			var s = "Unknown";
			switch ( CategoryCode )
			{
				case "1":
					s = "QUARTERBACKS";
					break;

				case "2":
					s = "RUNNING BACKS";
					break;

				case "3":
					s = "RECEIVERS";
					break;

				case "4":
					s = "KICKERS";
					break;

				default:
					s = "Not defined";
					break;
			}
			return s;
		}

		public decimal SortPoints
		{
			get
			{
				return ( ( 10.0M - Decimal.Parse( CategoryCode ) ) * 100.0M ) + ProjPts;
			}
		}

		public override string ToString()
		{
			return $"{Name,-35} {Opp,-10} {ProjPts,5}  {ActualPts}";
		}
	}

}
