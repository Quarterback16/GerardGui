using System;
using System.Diagnostics;
using NLog;
using RosterLib.Interfaces;

namespace RosterLib
{
   public class RosterGridReport
   {
      public Logger Logger { get; set; }

      public string Name { get; set; }

      public string Season { get; set; }

      public string FileOut { get; set; }

      public DateTime LastRun { get; set; }

      public IRunStorer MyRunStorer { get; set; }

      public Stopwatch Stopwatch { get; set; }

      public TimeSpan RunTime { get; set; }

      public IKeepTheTime TimeKeeper { get; set; }

	   public string TflFolder { get; set; }


      public RosterGridReport()
      {
         //  default to look at the current season
         Season = SeasonBoss.CurrentSeason();
         MyRunStorer = new DbfRunStorer();
         Stopwatch = new Stopwatch();
         Stopwatch.Start();
	      TflFolder = Config.TflFolder();
			SetLastRunDate();
      }

      public RosterGridReport(IKeepTheTime timekeeper )
      {
         //  default to look at the current season
         Season = SeasonBoss.CurrentSeason();
         MyRunStorer = new DbfRunStorer();
         Stopwatch = new Stopwatch();
         Stopwatch.Start();
         TimeKeeper = timekeeper;
			TflFolder = Config.TflFolder();
			SetLastRunDate();
		}

      public virtual string OutputFilename()
      {
         return string.Format("{0}{1}/{2}.htm", Utility.OutputDirectory(), Season, Name );
      }

      public virtual void RenderAsHtml()
      {
         Finish();
      }

      public virtual void RenderFullAsHtml()
      {
         Finish();
      }

      public virtual void Finish()
      {
         RunTime = Utility.StopTheWatch(Stopwatch, string.Format("Finished: {0}", Name));
         if ( string.IsNullOrEmpty( Name ) ) throw new ApplicationException( "Unnamed Report !!!" );
         MyRunStorer.StoreRun(Name, RunTime);
         LastRun = DateTime.Now;
      }

      public void SetLastRunDate()
      {
			if ( Name != null )
				LastRun = Utility.TflWs.GetLastRun(Name);
      }

      public string DoReport()
      {
         RenderAsHtml(); //  the old method that does the work
         Finish();
         return string.Format("Rendered {0} to {1}", Name, OutputFilename());
      }

      public string CheckLastRunDate()
      {
         var whyNot = string.Empty;
         var lastReport = LastRun;  //  this is null
	      var dataFile = string.Format( "{0}nfl//player.dbf", TflFolder );
         var dataDate = DataDate( dataFile );
         if (dataDate.Date < lastReport.Date)
            whyNot = string.Format("Last Run of {0} is later than the data date of {1}",
               lastReport, dataDate);

	      if (dataDate == new DateTime( 1, 1, 1 ))
		      whyNot += string.Format( "{0} not found", dataFile );

	      return whyNot;
      }

      /// <summary>
      ///   Push this out to the TimeKeeper or perhaps a more appropriate class?
      /// </summary>
      /// <returns></returns>
      private static DateTime DataDate( string dataFile )
      {
         var theDate = FileUtility.DateOf(dataFile);
         return theDate;
      }
   }
}