﻿using RosterLib.Interfaces;
using NLog;
using RosterLib;
using RosterLib.RosterGridReports;
using System;

namespace Butler.Models
{
   public class PlayOffTeamsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public PlayOffTeamsJob(IKeepTheTime timekeeper)
      {
         Name = "Playoff Teams";
         Console.WriteLine("Constructing {0} ...", Name);
         TimeKeeper = timekeeper;
         Logger = LogManager.GetCurrentClassLogger();

         Report = new PlayoffTeamsReport( TimeKeeper ) {Name = "Playoff Teams"};
      }

      public override string DoJob()
      {
         Logger.Info("Doing {0} job..............................................", Name);
         Report.RenderAsHtml(); //  the method that does the work
         Report.Finish();
         return string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
      }

		public override bool IsTimeTodo(out string whyNot)
		{
			base.IsTimeTodo(out whyNot);
			if (string.IsNullOrEmpty(whyNot))
			{
				if (TimeKeeper.IsItPeakTime())
					whyNot = "Peak time - no noise please";
			}
			if (!string.IsNullOrEmpty(whyNot))
				Logger.Info("Skipped {1}: {0}", whyNot, Name);
			return (string.IsNullOrEmpty(whyNot));
		}
   }
}
