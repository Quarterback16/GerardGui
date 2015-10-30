using Butler.Interfaces;
using Butler.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using RosterLib;
using RosterLib.Interfaces;

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
      public string Version { get; set; }

      public Logger Logger { get; set; }

      public bool Verbose { get; set; }

      public int Passes { get; set; }

      public BackgroundWorker MyWorker { get; set; }

      public int Pollinterval { get; set; }

      public List<Job> MyJobs { get; set; }

      public IKeepTheTime TimeKeeper { get; set; }

      public IHistorian Historian { get; set; }

      public Butler(string version)
      {
         Version = version;
         TimeKeeper = new TimeKeeper();
         Historian = new Historian();
         Logger = LogManager.GetCurrentClassLogger();
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

            // load jobs, in DEV u will work on one job at a time, but in PROD do em all
            MyJobs = new List<Job>
               {
                  new UpdateTflJob(),  //  look for nerd stick
                  new MediaJob(),  //  regular always
                  new UpdateTflJob(),  

#region Looking back on the games just played

                  new AssignRolesJob( TimeKeeper ),
                  new YahooXmlJob( TimeKeeper ),
                  new PerformanceReportJob( TimeKeeper ),
                  new DepthChartJob( TimeKeeper ),
                  new StatGridJob( TimeKeeper ),
                  new PlayOffTeamsJob( TimeKeeper ),

#endregion

#region  Looking forward to the upcoming games

                  new ProjectionsJob( TimeKeeper ), //  once in pre season then once a week regular - always
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

#endregion

                  new StartersJob( TimeKeeper ),  

#region Pre Season Jobs 
                  new PlayerCsvJob( TimeKeeper ),
                  new BalanceReportJob( TimeKeeper ), //  once off - pre season
                  new FreeAgentMarketJob( TimeKeeper ), //  regular - pre season
                  new StrengthOfScheduleJob( TimeKeeper ), //  once off - pre season
                  new PlayerReportsJob( TimeKeeper ),

#endregion

#region Regular Always jobs
                  new RunReportJob( TimeKeeper ),
                  new LogCleanupJob(), 
                  new TflDataBackupJob(),  
                  new DropboxCopyToReginaJob(), 
                  new MediaListsPublishJob()  
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
                     var outcome = job.DoJob();
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