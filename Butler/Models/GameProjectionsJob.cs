using RosterLib.Interfaces;
using RosterLib;
using System;
using Butler.Implementations;

namespace Butler.Models
{
   public class GameProjectionsJob : Job
   {
      public RosterGridReport Report { get; set; }

      public ISeasonScheduler SeasonScheduler { get; set; }

      public GameProjectionsJob(IKeepTheTime timeKeeper)
      {
         Name = "Game Projections";
         Console.WriteLine("Constructing {0} ...", Name);
         var week = string.Format( "{0:00}", timeKeeper.CurrentWeek( DateTime.Now ) );
         Report = new SeasonProjectionReport( timeKeeper.CurrentSeason( DateTime.Now ), week );
         TimeKeeper = timeKeeper;
         Logger = NLog.LogManager.GetCurrentClassLogger();
         IsNflRelated = true;
         SeasonScheduler = new SeasonScheduler();
      }

      public override string DoJob()
      {
         Logger.Info( "Doing {0} job..............................................", Name );
         // pre-req
         var preJob = new RankingsJob( TimeKeeper );
         var outcome = preJob.DoJob();
         Logger.Info("Rankings {0}", outcome );
         Report.RenderAsHtml(); //  the old method that does the work
         Report.Finish();
         return string.Format("Rendered {0} to {1}", Report.Name, Report.OutputFilename());
      }

      /// <summary>
      ///   Win Projections are done once in Preseason and then once every week during the season
      /// </summary>
      /// <param name="whyNot"></param>
      /// <returns></returns>
      public override bool IsTimeTodo(out string whyNot)
      {
         base.IsTimeTodo(out whyNot);
         if (string.IsNullOrEmpty(whyNot))
         {
            if ( !SeasonScheduler.ScheduleAvailable(TimeKeeper.CurrentSeason()))
            {
               whyNot = "The schedule is not yet available for " + TimeKeeper.CurrentSeason();
            }
         }
         if (string.IsNullOrEmpty(whyNot))
         {
            //  check if there is any new data
            whyNot = Report.CheckLastRunDate();
         }
         if (string.IsNullOrEmpty(whyNot))
         {
            if (TimeKeeper.IsItPeakTime())
               whyNot = string.Format( "{0:t} is peak time", DateTime.Now.TimeOfDay );
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }
   }
}