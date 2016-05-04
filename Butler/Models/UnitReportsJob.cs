using Butler.Interfaces;
using RosterLib;
using System;

namespace Butler.Models
{
	public class UnitReportsJob : Job
	{
		public RosterGridReport Report { get; set; }

		public IHistorian Historian { get; set; }

		public UnitReportsJob(IHistorian historian)
		{
			Name = "Unit Reports";
			Report = new UnitReport();
			Historian = historian;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
		}

		public override string DoJob()
		{
			Report.Season = Utility.CurrentSeason();
         return Report.DoReport();
      }

		public override bool IsTimeTodo(out string whyNot)
		{
			base.IsTimeTodo(out whyNot);
			if (string.IsNullOrEmpty(whyNot))
			{
            var regularity = 7;
            if (TimeKeeper.IsItPreseason())
               regularity += 14;
            var sevenDaysAgo = DateTime.Now.Subtract(new TimeSpan(regularity, 0, 0, 0)).Date;
				if (Historian.LastRun(Report).Date > sevenDaysAgo)
					whyNot = string.Format("Has been done less than {0} days ago",regularity);
			}
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return (string.IsNullOrEmpty(whyNot));
		}
	}
}