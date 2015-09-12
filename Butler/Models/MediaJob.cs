using Helpers.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Butler.Models
{
   public class MediaJob : Job
   {
      public string DownloadFolder { get; set; }

      public string MagazineCollectionFolder { get; set; }

      public string MagazineDestinationFolder { get; set; }

      public List<string> Candidates { get; set; }

      public MediaJob()
      {
         Console.WriteLine("Constructing MediaJob ...");
         Name = "Media Job";
         DownloadFolder = GetDownloadFolder();   //  "\\\\Vesuvius\\Downloads";
         MagazineCollectionFolder = GetMagazineFolder();
         MagazineDestinationFolder = GetMagazineDestinationFolder();
         Logger = LogManager.GetCurrentClassLogger();
      }

      public MediaJob(Logger logger)
      {
         Name = "Media Job";
         DownloadFolder = GetDownloadFolder();   //  "\\\\Vesuvius\\Downloads";
         Logger = logger;
      }

      public string GetDownloadFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get( "DownloadFolder" );
      }

      public string GetMagazineFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get( "MagazineCollectionFolder" );
      }

      public string GetMagazineDestinationFolder()
      {
         return System.Configuration.ConfigurationManager.AppSettings.Get("MagazineDestinationFolder");
      }

      public MediaJob(string folder)
      {
         Name = "Media Job";
         DownloadFolder = folder;
      }

      public override string DoJob()
      {
         var finishedMessage = string.Empty;
         try
         {
            Logger.Info("Doing {0} job..............................................", Name);
            
            var itemCount = 0;

            GetCandidates();
            var collector = new Collector();
            foreach (var f in Candidates)
            {
               itemCount++;
               Logger.Trace( string.Format( "Candidate: {0} ", f ) );
               var mi = new MediaInfo(f);
               if (!mi.IsValid) continue;

               if (!mi.HasValidExt()) continue;

               mi.MagazineFolder = MagazineCollectionFolder;
               mi.Analyse();

               if (mi.IsTV)
               {
                  if (!collector.HaveIt( mi )) continue;
                  var newFile = collector.AddToTvCollection(mi);
                  Logger.Info( string.Format( "  Adding TV     - {0} ", newFile));
               }
               else if (mi.IsMagazine)
               {
                  var newFile = collector.MoveToMagQueue(mi);
                  Logger.Info(string.Format("  Queing Mag    - {0} ", newFile));
               }
               else if (mi.IsBook)
               {
                  var newFile = collector.MoveToViewQueue(mi);
                  Logger.Info( string.Format( "  Queing Book   - {0} ", newFile));
               }
               else if ( mi.IsNfl )
               {
                  var newFile = collector.AddToNflCollection( mi );
                  Logger.Info( string.Format( "  Adding NFL    - {0} ", newFile ) );
               }
               else if ( mi.IsSoccer )
               {
                  var newFile = collector.AddToSoccerCollection( mi );
                  Logger.Info( string.Format( "  Adding Soccer - {0} ", newFile ) );
               }
               else if ( mi.IsMovie )
               {
                  var newFile = collector.AddToMovieCollection( mi );
                  Logger.Info( string.Format( "  Adding Movie  - {0} ", newFile ) );
               }
            }
            finishedMessage = string.Format("{0} items processed", itemCount);
         }
         catch (Exception ex)
         {
            Logger.Error(ex.StackTrace);
            throw;
         }
         Logger.Info("  {0}", finishedMessage);
         return finishedMessage;
      }

      public void GetCandidates()
      {
         try
         {
            Candidates = new List<string>();

            Candidates = Directory.GetFiles( DownloadFolder, "*.*", SearchOption.AllDirectories ).ToList();

            Logger.Trace( string.Format( "Found {0} files in folder:{1}", Candidates.Count, DownloadFolder ) );
         }
         catch ( IOException ex )
         {
            Logger.Error( string.Format( "Invalid DL folder {0} :{1}", DownloadFolder, ex.Message ) );
         }
      }

      public override bool IsTimeTodo(out string whyNot)
      {
         whyNot = string.Empty;
         if (OnHold()) whyNot = "Job is on hold";
         if (string.IsNullOrEmpty(whyNot))
         {
            //  Can't think of any reason yet why not as this is a constant job
            //  because ut is always running on the server
            //  maybe not work doing in peak time because of the ut scheduller
         }
         if ( !string.IsNullOrEmpty( whyNot ) )
            Logger.Info( "Skipped {1}: {0}", whyNot, Name );
         return (string.IsNullOrEmpty(whyNot));
      }
   }
}