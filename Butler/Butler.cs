using Butler.Interfaces;
using Butler.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using RosterLib;
using RosterLib.Interfaces;
using Helpers;

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

      public int Passes { get; private set; }

      public BackgroundWorker MyWorker { get; private set; }

      public int Pollinterval { get; set; }

      public List<Job> MyJobs { get; private set; }

      public IKeepTheTime TimeKeeper { get; private set; }

      public IHistorian Historian { get; private set; }

      public Butler(string version)
      {
         Version = version;
         TimeKeeper = new TimeKeeper();
         Historian = new Historian();
         Logger = LogManager.GetCurrentClassLogger();
			Logger.Info( "ver:{0}", Version);
      }

      public void ReportProgress(string message)
      {
         ReportProgress(message, 50);
      }

      public void ReportProgress(string message, int progress)
      {
         if (MyWorker != null)
            MyWorker.ReportProgress(progress, message);
      }

      public void Go(BackgroundWorker backgroundWorker1, DoWorkEventArgs e)
      {
         try
         {
            MyWorker = backgroundWorker1;
            var configReader = new ConfigReader();
            
            MyJobs = new List<Job>
               {
                  new DropBoxCopyTflToVesuviusJob(TimeKeeper),  //  get any new TFL data from dropbox
                  new MediaJob(),  //  regular always
                  new LogMailerJob( 
                     new MailMan2(configReader), 
                     new LogFileDetector(), 
                     configReader ),  //  once daily
                  new MediaMailerJob( 
                     new MailMan2(configReader), 
                     new MediaLogDetector(),
                     configReader),  //  once daily

#region Looking back on the games just played

                  new AssignRolesJob( TimeKeeper ),
                  new YahooXmlJob( TimeKeeper ),
                  new PerformanceReportJob( TimeKeeper ),
                  new DepthChartJob( TimeKeeper ),
                  new StatGridJob( TimeKeeper ),
                  new PlayOffTeamsJob( TimeKeeper ),
                  new UpdateActualsJob( TimeKeeper ),

#endregion

#region  Looking forward to the upcoming games

                  new GameProjectionsJob( TimeKeeper ), //  once in pre season then once a week regular - always
                  new GeneratePlayerProjectionsJob( TimeKeeper ),
                  new RookiesJob( TimeKeeper ),
                  new OutputProjectionsJob( Historian ),  //  needs game projections
                  new FantasyProjectionJob( TimeKeeper ),
                  new HotListsJob(),   //  regular always
                  new UnitReportsJob( Historian ),
                  new TeamCardsJob( TimeKeeper ),
                  new OldRosterGridJob( TimeKeeper ), //  regular - always
                  new DefensiveReportsJob( TimeKeeper ),
                  new SuggestedLineupsJob( TimeKeeper ),
                  new PickupChartJob( TimeKeeper ),
                  new StartersJob( TimeKeeper ),

#endregion


#region Regular Always jobs
                  new RunReportJob( TimeKeeper ),
                  new LogCleanupJob(),
                  new TflDataBackupJob(),
                  new DropboxCopyToReginaJob( TimeKeeper),
                  new MediaListsPublishJob(),
#endregion


#region Pre Season Jobs -- Long running so last in the job order
                  new PlayerCsvJob( TimeKeeper ),
                  new BalanceReportJob( TimeKeeper ), //  once off - pre season
                  new FreeAgentMarketJob( TimeKeeper ), //  regular - pre season
                  new StrengthOfScheduleJob( TimeKeeper ), //  once off - pre season
                  new PlayerReportsJob( TimeKeeper )
#endregion

               };

            if (Passes == 0)
               ReportProgress(
                  string.Format("{0} - {1} jobs defined -Starting...",
                                 Version, MyJobs.Count), ButlerConstants.ReportInTextArea);

            while (true)
            {
               Passes++;

               Logger.Info( "-------------------------------------------------------------------------------------" );
               foreach ( var job in MyJobs )
               {
                  string whyNot;
                  if (job.IsTimeTodo(out whyNot))
                  {
                     ReportProgress(
                        string.Format("Doing job {0}", job.Name), ButlerConstants.ReportInTextArea);
                     var outcome = job.Execute();
                     ReportProgress(outcome, ButlerConstants.ReportInTextArea);
                  }
                  else
                     ReportProgress(
                        string.Format("Job skipped {0} - {1}", job.Name, whyNot),
                        ButlerConstants.ReportInTextArea);
               }
               Logger.Info( "=====================================================================================" );

               ReportProgress(string.Format(
                  "Pass Number {0} done - next pass ({1}) {2:HH:mm}",
                  Passes, Pollinterval, DateTime.Now.AddMinutes(Pollinterval)));
               Thread.Sleep(Pollinterval * 60 * 1000); //  minutes

               if (!MyWorker.CancellationPending) continue;

               e.Cancel = true;
               break;
            }
         }
         catch (Exception ex)
         {
            if (Logger == null) Logger = LogManager.GetCurrentClassLogger();
            Logger.Error(ex.Message);
            Logger.Error(ex.StackTrace);
            throw;
         }
      }
   }
}