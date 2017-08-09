using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class StartersJob : Job
	{
		public RosterGridReport Report { get; set; }

		public StartersJob( IKeepTheTime timeKeeper ) : base()
		{
			Name = nameof( Starters );
			TimeKeeper = timeKeeper;
			Report = new Starters( TimeKeeper );
			Logger = NLog.LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
		}

		public override string DoJob()
		{
			return Report.DoReport();
		}

		public override bool IsTimeTodo( out string whyNot )
		{
			base.IsTimeTodo( out whyNot );

			if ( string.IsNullOrEmpty( whyNot ) )
			{
#if !DEBUG
            if (TimeKeeper.IsItPeakTime())
               whyNot = "Peak time - no noise please";
            if (string.IsNullOrEmpty(whyNot))
            {
               //  check if there is any new data
               whyNot = Report.CheckLastRunDate();
            }
#endif
			}
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}