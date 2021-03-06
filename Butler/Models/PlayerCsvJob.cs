using NLog;
using RosterLib;
using RosterLib.Helpers;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class PlayerCsvJob : Job
	{
		public RosterGridReport Report { get; set; }

		public PlayerCsvJob(
            IKeepTheTime timeKeeper )
		{
			Name = "Players CSV";
			TimeKeeper = timeKeeper;
			var rpt = new PlayerCsv(
                timeKeeper,
                new AdpMaster(
                    $"{Utility.OutputDirectory()}XML\\adp.csv"))
			{
				DoProjections = TimeKeeper.IsItPreseason()
			};
			Report = rpt;
			IsNflRelated = true;
			Logger = LogManager.GetCurrentClassLogger();
		}

		public override bool IsTimeTodo(
            out string whyNot )
		{
            base.IsTimeTodo( out whyNot );
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				//  Check that you have already done it for today (happens in Dev a lot)
				var theDate = FileUtility.DateOf( Report.OutputFilename() );
				if ( !TimeKeeper.IsItPreseason() )
					whyNot = "Its not Pre Season";

				if ( TimeKeeper.IsItPeakTime() )
					whyNot = "Peak time - no noise please";
			}
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}

		public override string DoJob()
		{
			var finishMessage = Report.DoReport();
			Logger.Info( "  {0}", finishMessage );
			return finishMessage;
		}
	}
}