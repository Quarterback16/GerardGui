using Helpers.Interfaces;
using RosterLib;
using RosterLib.Interfaces;
using System;

namespace Butler.Models
{
	public class PlayerReportsJob : Job
	{
		public RosterGridReport Report { get; set; }

		private const string K_PlayerReportsTodo = "PlayerReportsToDo";

		public PlayerReportsJob( IKeepTheTime timeKeeper, IConfigReader configReader ) : base( timeKeeper )
		{
			var reportsToDo = configReader.GetSetting( K_PlayerReportsTodo );
			Name = "Player Reports";
			Report = new PlayerCareerReport( TimeKeeper, Int32.Parse(reportsToDo) );
			TimeKeeper = timeKeeper;
			Logger = NLog.LogManager.GetCurrentClassLogger();
			IsNflRelated = true;
			Logger.Info( $"Doing {reportsToDo} reports" );
		}

		public override string DoJob()
		{
			return Report.DoReport();
		}

		//  new business logic as to when to do the job
		public override bool IsTimeTodo( out string whyNot )
		{
			whyNot = string.Empty;
			if ( OnHold() ) whyNot = "Job is on hold";
			if ( !string.IsNullOrEmpty( whyNot ) ) return ( string.IsNullOrEmpty( whyNot ) );
			if ( TimeKeeper.IsItPreseason() )
			{
				if ( TimeKeeper.IsItPeakTime() )
					whyNot = "Peak time - no noise please";
			}
			else
				whyNot = "Its not preseason";

			if ( string.IsNullOrEmpty( whyNot ) ) return ( string.IsNullOrEmpty( whyNot ) );

			var msg = $"Skipped {Name}: {whyNot}";
			Console.WriteLine();
			Logger.Info( msg );
			return ( string.IsNullOrEmpty( whyNot ) );
		}
	}
}