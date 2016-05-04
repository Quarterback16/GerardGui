using RosterLib.Interfaces;
using RosterLib;
using System;
using NLog;

namespace Butler.Models
{
	public class FreeAgentMarketJob : Job
	{
		public RosterGridReport Report { get; set; }

		public FreeAgentMarketJob(IKeepTheTime timeKeeper)
		{
			Name = "Free Agent Report";
			Report = new FaMarket { Name = Name, Season = timeKeeper.Season };
			TimeKeeper = timeKeeper;
         IsNflRelated = true;
         Logger = LogManager.GetCurrentClassLogger();
		}

		public override string DoJob()
		{
			Report.RenderAsHtml();
			Report.Finish();
			var finishedMessage = string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
         Logger.Info( "  {0}", finishedMessage );
         return finishedMessage;
		}

		public override bool IsTimeTodo(out string whyNot)
		{
		   base.IsTimeTodo(out whyNot);
		   if (!string.IsNullOrEmpty( whyNot )) return ( string.IsNullOrEmpty( whyNot ) );

		   //  If it is preseason report  do it assuming Players are being constantly updated
		   if (!TimeKeeper.IsItPreseason())
		      whyNot = "Not Preseason";
		   else
		   {
		      //  Chck that you have already done it for today (happens in Dev a lot)
		      var theDate = FileUtility.DateOf(Report.OutputFilename());
		      if (theDate.Date.Equals(DateTime.Now.Date))
		         whyNot = "Already done today";
		   }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
		   return (string.IsNullOrEmpty(whyNot));
		}
	}
}