using Butler.Interfaces;
using Butler.Models;
using Helpers;
using NLog;
using RosterLib;
using RosterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Butler
{
	/// <summary>
	///   The butler is tasked to do all the jobs and his purpose is to save me time.
	///   The are a class of job regarded as housekeeping that is well suited to Gerard.
	///   Jobs can be anything, computer jobs, TFL NFL jobs, maybe even paying bills
	///   and sending emails.
	///   This jobs will be physically performed on the Always on Home Server, so they can be
	///   triggered anytime.
	///   Jobs should be very granular and not rely too much on previous jobs.  So evolve jobs
	///   that do many things into jobs that do just one thing.  Smaller jobs are better.
	///   There will be some "infrastructutal singletons" that jobs can depend on, like the TimeKeeper
	///   and the Historian.
	///   The TimeKeeper is to help with deciding whther its the right time to be doing a job.
	///   The Historian keeps track of when jobs were done and how they went.
	/// </summary>
	public class Butler
	{
		public string Version { get; private set; }

		public Logger Logger { get; private set; }

		public bool Verbose { get; set; }

		public bool AutoStart { get; set; }

		public int Passes { get; private set; }

		public int PassQuota { get; set; }

		public BackgroundWorker MyWorker { get; private set; }

		public int Pollinterval { get; set; }

		public List<Job> MyJobs { get; private set; }

		public IKeepTheTime TimeKeeper { get; private set; }

		public IHistorian Historian { get; private set; }

		public Butler( string version, IKeepTheTime timekeeper )
		{
			Version = version;
			if ( timekeeper != null )
				TimeKeeper = timekeeper;
			else
				TimeKeeper = new TimeKeeper( null );

			Historian = new Historian();
			Logger = LogManager.GetCurrentClassLogger();
			Logger.Info( "ver:{0}", Version );
		}

		public void ReportProgress( string message )
		{
			ReportProgress( message, 50 );
		}

		public void ReportProgress( string message, int progress )
		{
			if ( MyWorker != null )
				MyWorker.ReportProgress( progress, message );
		}

		public void Go( BackgroundWorker backgroundWorker1, DoWorkEventArgs e )
		{
			try
			{
				MyWorker = backgroundWorker1;
				var configReader = new ConfigReader();

				MyJobs = new List<Job>();

				MyJobs.Add( new DropBoxCopyTflToVesuviusJob( TimeKeeper ) ); //  get any new TFL data from dropbox
				MyJobs.Add( new LogMailerJob( new MailMan2( configReader ), new LogFileDetector()) );  //  once daily
				MyJobs.Add( new MediaMailerJob( 
					new MailMan2( configReader ), new MediaLogDetector(), configReader, TimeKeeper ) );

				#region Looking back on the games just played

				MyJobs.Add( new AssignRolesJob( TimeKeeper ) );  //  sets the ROLE on players based on actual stats
				MyJobs.Add( new YahooXmlJob( TimeKeeper ) );
				MyJobs.Add( new PerformanceReportJob( TimeKeeper ) );
				MyJobs.Add( new FantasyReportJob( TimeKeeper ) );
				MyJobs.Add( new DepthChartJob( TimeKeeper ) );
				MyJobs.Add( new StatGridJob( TimeKeeper ) );
				MyJobs.Add( new PlayOffTeamsJob( TimeKeeper ) );
				MyJobs.Add( new UpdateActualsJob( TimeKeeper ) );
				MyJobs.Add( new UpdateTeamActualsJob( TimeKeeper ) );
				MyJobs.Add( new TopDogReportJob( TimeKeeper ) );
				MyJobs.Add( new GameSummariesJob( TimeKeeper ) );

				#endregion Looking back on the games just played

				#region Looking forward to the upcoming games

				MyJobs.Add( new GameProjectionsJob( TimeKeeper ) ); //  once in pre season then once a week regular - always
				MyJobs.Add( new GeneratePlayerProjectionsJob( TimeKeeper ) );
				MyJobs.Add( new GameProjectionReportsJob( TimeKeeper ) );
				MyJobs.Add( new RookiesJob( TimeKeeper ) );
				MyJobs.Add( new OutputProjectionsJob( TimeKeeper, Historian ) );  //  needs game projections
				MyJobs.Add( new FantasyProjectionJob( TimeKeeper ) );
				MyJobs.Add( new HotListsJob( TimeKeeper ) );
				MyJobs.Add( new UnitReportsJob( TimeKeeper, Historian ) );
				MyJobs.Add( new TeamCardsJob( TimeKeeper ) );
				MyJobs.Add( new OldRosterGridJob( TimeKeeper ) );
				MyJobs.Add( new DefensiveReportsJob( TimeKeeper ) );
				MyJobs.Add( new SuggestedLineupsJob( TimeKeeper ) );
				MyJobs.Add( new PickupChartJob( TimeKeeper ) );
				MyJobs.Add( new PreviousPickupChartJob( TimeKeeper, Historian ) );
				MyJobs.Add( new StartersJob( TimeKeeper ) );
				MyJobs.Add( new PointsAllowedReportJob( TimeKeeper ) );

				#endregion Looking forward to the upcoming games

				#region Regular Always jobs

				MyJobs.Add( new RunReportJob( TimeKeeper ) );
				MyJobs.Add( new LogCleanupJob() );
				MyJobs.Add( new TflDataBackupJob() );
				MyJobs.Add( new MediaListsPublishJob() );

				#endregion Regular Always jobs

				#region Pre Season Jobs -- Long running so last in the job order

				MyJobs.Add( new PlayerCsvJob( TimeKeeper ) );
				MyJobs.Add( new BalanceReportJob( TimeKeeper ) ); //  once off - pre season
				MyJobs.Add( new DeletePlayerReportsJob( TimeKeeper ) ); //  once off - pre season
				MyJobs.Add( new RetirePlayersJob( TimeKeeper ) ); //  once off - pre season
				MyJobs.Add( new FreeAgentMarketJob( TimeKeeper ) ); //  regular - pre season
				MyJobs.Add( new StrengthOfScheduleJob( TimeKeeper ) ); //  once off - pre season
				MyJobs.Add( new PlayerReportsJob( TimeKeeper, configReader ) );
				MyJobs.Add( new PositionReportJob( TimeKeeper ) );

				#endregion Pre Season Jobs -- Long running so last in the job order

				MyJobs.Add( new DropboxCopyToReginaJob( TimeKeeper,
				  "d:\\shares\\public\\dropbox\\gridstat\\{0}",
				  "\\\\Regina\\web\\medialists\\dropbox\\gridstat\\{0}"
				  ) );

				MyJobs.Add( new DropboxCopyToReginaJob( TimeKeeper,
				  "d:\\shares\\public\\dropbox\\gridstat\\tfl-out",
				  "\\\\Regina\\web\\medialists\\dropbox\\gridstat\\tfl-out"
				  ) );

				MyJobs.Add( new DropBoxCopyFromDeLoochJob( TimeKeeper,
					"\\\\DeLooch\\users\\steve\\dropbox\\lists\\",
					"d:\\shares\\public\\dropbox\\lists\\"
				) );

				MyJobs.Add( new MediaJob() );  //  regular always lucky last

				if ( Passes == 0 )
					ReportProgress(
						$"{Version} - {MyJobs.Count} jobs defined -Starting...", ButlerConstants.ReportInTextArea );

				while ( true )
				{
					Passes++;

					Logger.Info( "-------------------------------------------------------------------------------------" );
					foreach ( var job in MyJobs )
					{
						if ( job.IsTimeTodo( out string whyNot ) )
						{
							ReportProgress(
								$"Doing job {job.Name}", ButlerConstants.ReportInTextArea );
							var outcome = job.Execute();
							ReportProgress( outcome, ButlerConstants.ReportInTextArea );
						}
						else
							ReportProgress(
								$"Job skipped {job.Name} - {whyNot}",
							   ButlerConstants.ReportInTextArea );
					}
					Logger.Info( "=====================================================================================" );

					ReportProgress( $"Pass Number {Passes} done - next pass ({Pollinterval}) {DateTime.Now.AddMinutes( Pollinterval ):HH:mm}");
					if ( Passes >= PassQuota ) break;
					Thread.Sleep( Pollinterval * 60 * 1000 ); //  <pollInterval> hours

					if ( !MyWorker.CancellationPending ) continue;

					e.Cancel = true;
					break;
				}
			}
			catch ( Exception ex )
			{
				if ( Logger == null ) Logger = LogManager.GetCurrentClassLogger();
				Logger.Error( ex.Message );
				Logger.Error( ex.StackTrace );
				throw;
			}
		}
	}
}