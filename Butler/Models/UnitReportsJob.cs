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
			Console.WriteLine("Constructing {0} ...", Name);
			Report = new UnitReport();
			Historian = historian;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
		}

		public override string DoJob()
		{
         Logger.Info( "Doing {0} job..............................................", Name );
			Report.Season = Utility.CurrentSeason();
			Report.RenderAsHtml(); //  the old method that does the work
			return string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
		}

		//  new business logic as to when to do the job
		public override bool IsTimeTodo(out string whyNot)
		{
			base.IsTimeTodo(out whyNot);
			if (string.IsNullOrEmpty(whyNot))
			{
				var sevenDaysAgo = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)).Date;
				if (Historian.LastRun(Report).Date > sevenDaysAgo)
					whyNot = "Has been done in the last week";
			}
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
			return (string.IsNullOrEmpty(whyNot));
		}
	}
}