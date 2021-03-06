using NLog;
using RosterLib;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class YahooXmlJob : ReportJob
	{
		public bool FullSeason { get; set; }

		public YahooXmlJob(
            IKeepTheTime timekeeper )
		{
			Name = "Yahoo Xml job";
			Report = new YahooMasterGenerator(
                FullSeason,
                timekeeper );
			TimeKeeper = timekeeper;
			IsNflRelated = true;
			Logger = LogManager.GetCurrentClassLogger();
		}

		public override bool IsTimeTodo(
            out string whyNot )
		{
            base.IsTimeTodo( out whyNot );
			if ( string.IsNullOrEmpty( whyNot ) )
			{
				if ( TimeKeeper.IsItPeakTime() )
					whyNot = "Peak time - no noise please";
			}
			if ( TimeKeeper.IsItTuesday() )
				whyNot = "Not on Tuesdays";
			if ( !string.IsNullOrEmpty( whyNot ) )
				Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}