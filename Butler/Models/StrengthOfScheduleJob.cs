using RosterLib.Interfaces;
using RosterLib;
using System;
using NLog;
using Butler.Implementations;
using RosterLib.Helpers;

namespace Butler.Models
{
	public class StrengthOfScheduleJob : Job
	{
      public ISeasonScheduler SeasonScheduler { get; set; }

		public StrengthOfScheduleJob(IKeepTheTime timeKeeper)
		{
			Name = "Strength of Schedule Report";
			TimeKeeper = timeKeeper;
         IsNflRelated = true;
         Logger = LogManager.GetCurrentClassLogger();
         SeasonScheduler = new SeasonScheduler();
		}

		public override string DoJob()
		{
			var br = new StrengthOfSchedule( TimeKeeper.CurrentSeason( DateTime.Now ) );
			br.RenderAsHtml();

			return string.Format("Rendered {0} to {1}", br.Name, br.OutputFilename());
		}

		public override bool IsTimeTodo(out string whyNot)
		{
		   base.IsTimeTodo(out whyNot);
		   if (!string.IsNullOrEmpty( whyNot )) return ( string.IsNullOrEmpty( whyNot ) );

         if (string.IsNullOrEmpty(whyNot))
         {
            if (!SeasonScheduler.ScheduleAvailable(TimeKeeper.CurrentSeason()))
            {
               whyNot = "The schedule is not yet available for " + TimeKeeper.CurrentSeason();
            }
         }
         if (string.IsNullOrEmpty(whyNot))
         {
            if (!TimeKeeper.IsItPreseason())
               whyNot = "Not Preseason";
         }
#if ! DEBUG
		   if (string.IsNullOrEmpty( whyNot ))
		   {
            //  Is it already done?
            var rpt = new StrengthOfSchedule();
            var outFile = rpt.OutputFilename();
            if ( System.IO.File.Exists( outFile ) )
		         whyNot = string.Format( "{0} exists already", outFile );
		   }
		   Console.WriteLine( "Job:Reason for not doing>{0}", whyNot );
#endif
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
		   return (string.IsNullOrEmpty(whyNot));
		}
	}
}