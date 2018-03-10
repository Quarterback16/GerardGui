using Butler.Helpers;
using Butler.Interfaces;
using NLog;
using RosterLib;
using RosterLib.Interfaces;
using System;
using System.Diagnostics;

namespace Butler.Models
{
	public class Job : IJob
	{
		public string Name { get; set; }

		public Logger Logger { get; set; }

		public bool IsNflRelated { get; set; }

		public IKeepTheTime TimeKeeper { get; set; }

		public Stopwatch Stopwatch { get; set; }

		public IHoldList MyHoldList { get; set; }

		public TimeSpan ElapsedTimeSpan { get; set; }

		public Job()
		{
			MyHoldList = new HoldList();
			MyHoldList.LoadFromXml( ".//xml//hold-jobs.xml" );
			TimeKeeper = new TimeKeeper( null );
			Logger = LogManager.GetCurrentClassLogger();
		}

		public Job( IKeepTheTime timekeeper )
		{
			MyHoldList = new HoldList();
			MyHoldList.LoadFromXml( ".//xml//hold-jobs.xml" );
			TimeKeeper = timekeeper;
		}

		public virtual bool IsTimeTodo( out string whyNot )
		{
			whyNot = string.Empty;
			if ( OnHold() ) whyNot = "Job is on hold";

			if ( !string.IsNullOrEmpty( whyNot ) ) return string.IsNullOrEmpty( whyNot );

			if ( !IsNflRelated ) return string.IsNullOrEmpty( whyNot );

            if (!TimeKeeper.IsItPreseason()
                && !TimeKeeper.IsItRegularSeason()
                && !TimeKeeper.IsItPostSeason())
            {
                whyNot = "Season is over";
            }

            if ( !string.IsNullOrEmpty( whyNot ) )
			{
				if ( TimeKeeper.IsItMondayMorning() )
				{
					whyNot = "Games in progress";
				}
			}
			if ( !string.IsNullOrEmpty( whyNot ) )
			{
#if DEBUG
				Console.WriteLine( $"Base:Reason for not doing > {whyNot}" );
#endif
				Logger.Info( $"  Not time because : {whyNot}" );
			}

			return string.IsNullOrEmpty( whyNot );
		}

		public string Execute()
		{
			SetupJob();
			// Implement the work stuff ur self with an override,
			// but we want "standard" setups and teardowns
			var result = DoJob();

			TeardownJob();

			return result;
		}

		public virtual string DoJob()
		{
			//  the meat in the sandwich
			return string.Empty;
		}

		public void StartRun()
		{
			if ( Stopwatch == null )
				Stopwatch = new Stopwatch();
			Stopwatch.Start();
		}

		private void SetupJob()
		{
			Logger.Info( "Doing {0} job..............................................", Name );

			if ( Stopwatch == null )
				Stopwatch = new Stopwatch();
			Stopwatch.Start();
		}

		/// <summary>
		///   Records the run
		/// </summary>
		public void StopRun()
		{
			ElapsedTimeSpan = Utility.StopTheWatch( Stopwatch, $"Finished Job: {Name}");
			var runStorer = new DbfRunStorer();
			runStorer.StoreRun( Name, ElapsedTimeSpan, nameof( Job ) );
			LogElapsedTime( ElapsedTimeSpan );
		}

		public void TeardownJob()
		{
			ElapsedTimeSpan = Utility.StopTheWatch( Stopwatch, $"Finished Job: {Name}");
			LogElapsedTime( ElapsedTimeSpan );
		}

		public void LogElapsedTime( TimeSpan ts )
		{
			var elapsed = ts.ToString( @"hh\:mm\:ss" );
			Logger.Info( $"  Job: {Name} took {elapsed}");
		}

		public DateTime LastDone()
		{
			var lastDone = new DateTime( 1, 1, 1 );
			try
			{
				lastDone = Utility.TflWs.GetLastRun( Name );
			}
			catch ( Exception ex )
			{
				Logger.Error( "Could not get last run for " + Name + "  " + ex.Message );
			}

			return lastDone;
		}

		public virtual bool OnHold()
		{
			var onHold = MyHoldList.Contains( Name );
			return onHold;
		}
	}
}