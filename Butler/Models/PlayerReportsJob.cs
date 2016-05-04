using RosterLib;
using System;
using RosterLib.Interfaces;

namespace Butler.Models
{
	public class PlayerReportsJob : Job
	{
		public RosterGridReport Report { get; set; }

      public PlayerReportsJob( IKeepTheTime timeKeeper )
		{
			Name = "Player Reports";
         Report = new PlayerCareerReport( TimeKeeper.CurrentSeason( DateTime.Now ) );
         TimeKeeper = timeKeeper;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
		}

		public override string DoJob()
		{
         return Report.DoReport();
		}

		//  new business logic as to when to do the job
		public override bool IsTimeTodo(out string whyNot)
		{
			whyNot = string.Empty;
			if (OnHold()) whyNot = "Job is on hold";
		   if (!string.IsNullOrEmpty( whyNot )) return ( string.IsNullOrEmpty( whyNot ) );
		   if (TimeKeeper.IsItPreseason())
		   {
		      if (TimeKeeper.IsItPeakTime())
		         whyNot = "Peak time - no noise please";
		   }
		   else
		      whyNot = "Its not preseason";

		   if (string.IsNullOrEmpty( whyNot )) return ( string.IsNullOrEmpty( whyNot ) );

		   var msg = string.Format( "Skipped {1}: {0}", whyNot, Name );
		   Console.WriteLine( );
		   Logger.Info( msg );
		   return (string.IsNullOrEmpty(whyNot));
		}
	}
}