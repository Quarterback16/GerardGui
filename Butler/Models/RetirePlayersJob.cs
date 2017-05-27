using RosterLib;
using RosterLib.Interfaces;
using RosterLib.RosterGridReports;

namespace Butler.Models
{
	public class RetirePlayersJob : Job
	{
		public RosterGridReport Report { get; set; }

		public RetirePlayersJob( IKeepTheTime timeKeeper ) : base( timeKeeper )
		{
			Name = "Retire Players";
			Report = new RetirePlayersReport( TimeKeeper );
			TimeKeeper = timeKeeper;
			Logger = NLog.LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
		}

		public override string DoJob()
		{
			return Report.DoReport();
		}

		//  new business logic as to when to do the job
		public override bool IsTimeTodo( out string whyNot )
		{
			base.IsTimeTodo( out whyNot );
			if ( !string.IsNullOrEmpty( whyNot ) ) return ( string.IsNullOrEmpty( whyNot ) );

			//  Is it already done?
			var rpt = new RetirePlayersReport( TimeKeeper );
			var outFile = rpt.OutputFilename();
			if ( System.IO.File.Exists( outFile ) )
				whyNot = $"{outFile} exists already";
			else
			{
				if ( ! TimeKeeper.IsItPreseason() )
					whyNot = "Not Preseason";
			}
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}
