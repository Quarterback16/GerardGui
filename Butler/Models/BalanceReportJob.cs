using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class BalanceReportJob : Job
	{
		private readonly IKeepTheTime _timeKeeper;

		public BalanceReportJob( IKeepTheTime timeKeeper )
		{
			Name = "Balance Report";
			_timeKeeper = timeKeeper;
			IsNflRelated = true;
			Logger = LogManager.GetCurrentClassLogger();
		}

		public override string DoJob()
		{
			var br = new BalanceReport( _timeKeeper );
			br.Render();
			return $"Rendered {br.Name} to {br.OutputFilename()}";
		}

		public override bool IsTimeTodo( out string whyNot )
		{
			base.IsTimeTodo( out whyNot );
			if ( !string.IsNullOrEmpty( whyNot ) )
                return ( string.IsNullOrEmpty( whyNot ) );

			//  Is it already done?
			var rpt = new BalanceReport( _timeKeeper );
			var outFile = rpt.OutputFilename();
			if ( System.IO.File.Exists( outFile ) )
				whyNot = $"{outFile} exists already";
			else
			{
				if ( !_timeKeeper.IsItPreseason() )
					whyNot = "Not Preseason";
			}
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}