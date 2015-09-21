using Butler.Helpers;
using Butler.Interfaces;
using NLog;
using RosterLib;
using System;
using System.Diagnostics;
using RosterLib.Interfaces;

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

		public Job()
		{
			MyHoldList = new HoldList();
			MyHoldList.LoadFromXml(".//xml//hold-jobs.xml");
         TimeKeeper = new TimeKeeper();
		}

      public Job( IKeepTheTime timekeeper)
      {
         MyHoldList = new HoldList();
         MyHoldList.LoadFromXml( ".//xml//hold-jobs.xml" );
         TimeKeeper = timekeeper;
      }

		public virtual bool IsTimeTodo(out string whyNot)
		{
			whyNot = string.Empty;
         if (OnHold()) whyNot = "Job is on hold";

		   if (!string.IsNullOrEmpty( whyNot )) return string.IsNullOrEmpty( whyNot );

		   if (!IsNflRelated) return string.IsNullOrEmpty( whyNot );

		   if (TimeKeeper.IsItPostSeason())
		      whyNot = "Its the Post Season";

#if DEBUG
         Console.WriteLine( "Base:Reason for not doing>{0}", whyNot );
#endif

		   return string.IsNullOrEmpty(whyNot);
		}

		public virtual string DoJob()
		{
			// Implement it ur self with an override
			return string.Empty;
		}

		public void StartRun()
		{
			if (Stopwatch == null )
				Stopwatch = new Stopwatch();
			Stopwatch.Start();
		}

		/// <summary>
		///   Records the run
		/// </summary>
		public void StopRun()
		{
			var ts = Utility.StopTheWatch(Stopwatch, string.Format("Finished Job: {0}", Name));
			var runStorer = new DbfRunStorer();
			runStorer.StoreRun(Name, ts, "Job");
		}

		public DateTime LastDone()
		{
         DateTime lastDone = new DateTime(1,1,1);
         try
         {
            lastDone = Utility.TflWs.GetLastRun(Name);
         }
         catch (Exception ex )
         {
            Logger.Error("Could not get last run for " + Name + "  " + ex.Message );
         }

		   return lastDone;
		}

	   public bool OnHold()
		{
	      var onHold = MyHoldList.Contains(Name);
	      return onHold;
		}
	}
}