using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class OldRosterGridJob : Job
	{
		public RosterGridReport Report { get; set; }

		public OldRosterGridJob( IKeepTheTime timeKeeper )
		{
			Name = "Old Roster Grid";
			TimeKeeper = timeKeeper;
			Report = new OldRosterGridReports( TimeKeeper );
			Logger = NLog.LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
		}

		public override string DoJob()
		{
			return Report.DoReport();
		}

		public override bool IsTimeTodo( out string whyNot )
		{
			whyNot = string.Empty;
			base.IsTimeTodo( out whyNot );

			if ( string.IsNullOrEmpty( whyNot ) )
			{
				//  check if there is any new data
				whyNot = Report.CheckLastRunDate();
				if ( TimeKeeper.IsItPeakTime() )
					whyNot = "Peak time - no noise please";
			}
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}