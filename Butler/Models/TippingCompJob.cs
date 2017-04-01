using RosterLib.Interfaces;
using RosterLib;

namespace Butler.Models
{
	public class TippingCompJob : Job
	{
		public TippingCompJob( IKeepTheTime timeKeeper )
         : base()
      {
			Name = "Tipping Comp";
			TimeKeeper = timeKeeper;
			Logger = NLog.LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
		}

		public override string DoJob()
		{
			var tippingComp = new TippingController();
			tippingComp.Index( season: TimeKeeper.Season );
			return tippingComp.OutputFilename;
		}

		public override bool IsTimeTodo( out string whyNot )
		{
			base.IsTimeTodo( out whyNot );
			if ( TimeKeeper.IsItPreseason() )
				whyNot = "Comp has not started yet";
			else if ( TimeKeeper.IsItPeakTime() )
				whyNot = "Peak time - no noise please";
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}
