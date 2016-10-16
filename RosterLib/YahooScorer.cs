using Helpers;
using System;
using System.Data;

namespace RosterLib
{
   /// <summary>
   /// Applys the Yahoo scoring rules to a player after the stats have been recorded.
   /// </summary>
   public class YahooScorer : IRatePlayers
   {
      public IPlayerGameMetricsDao PgmDao { get; set; }

      public bool WeekHasPassed { get; set; }

      public string GameKey { get; set; }

	   public NFLGame Game { get; set; }

      public YahooScorer( NFLWeek week )
      {
         Name = "Yahoo Scorer";
         Week = week;
         PgmDao = new DbfPlayerGameMetricsDao();
      }

      #region IRatePlayers Members

      public bool ScoresOnly { get; set; }

      public Decimal RatePlayer( NFLPlayer plyr, NFLWeek week )
      {
         // Points for Scores and points for stats
         if ( week.WeekNo.Equals( 0 ) ) return 0;

         if ( plyr.TeamCode == null )
         {
            Console.WriteLine("{0} has a null teamcode", plyr);
            return 0;
         }
         GameKey = Week.GameCodeFor( plyr.TeamCode );
         if (string.IsNullOrEmpty(GameKey)) return 0;

			Game = new NFLGame(GameKey);
	      WeekHasPassed = Game.Played();

         Week = week;  //  set the global week, other wise u will get the same week all the time
         plyr.Points = 0;  //  start from scratch

         #region Passing

         //  4 pts for a Tdp
         var tdpPts = PointsFor( plyr, 4, Constants.K_SCORE_TD_PASS, id: "2" );
         plyr.Points += tdpPts;
#if DEBUG
         Utility.Announce( string.Format(
            "{0} has {1} points for Tdp", plyr.PlayerName, tdpPts ) );
#endif
         //  2 pts for a PAT pass
         var ptsForPATpasses = PointsFor( plyr, 2, Constants.K_SCORE_PAT_PASS, id: "2" );
         plyr.Points += ptsForPATpasses;
#if DEBUG
         Utility.Announce( string.Format(
            "{0} has {1} points for PAT passes", plyr.PlayerName, ptsForPATpasses ) );
#endif
         //  1 pt / 25 YDp
         var ptsForYDp = PointsForStats( plyr: plyr, increment: 1, forStatType: Constants.K_STATCODE_PASSING_YARDS, divisor: 25.0M );
         plyr.Points += ptsForYDp;
#if DEBUG
         Utility.Announce( string.Format(
				"{0} has {1} points for {2} YDp", plyr.PlayerName, ptsForYDp, plyr.ProjectedYDp ));
#endif
         //  -2 pts for an Interception
         var ptsForInts = PointsForStats(
            plyr: plyr, increment: -1, forStatType: Constants.K_STATCODE_INTERCEPTIONS_THROWN, divisor: 1.0M );
         plyr.Points += ptsForInts;
#if DEBUG
         Utility.Announce( string.Format( "{0} has {1} points for Interceptions", plyr.PlayerName, ptsForInts ) );
#endif

         #endregion Passing

         #region Catching

         //  6 pts for a TD catch
         var ptsForTDcatches = PointsFor( plyr, 6, Constants.K_SCORE_TD_PASS, id: "1" );
         plyr.Points += ptsForTDcatches;
#if DEBUG
         Utility.Announce( string.Format(
            "{0} has {1} points for TD catches", plyr.PlayerName, ptsForTDcatches ) );
#endif
         //  2 points for a 2 point conversion
         var ptsForPATcatches = PointsFor( plyr, 2, Constants.K_SCORE_PAT_PASS, id: "1" );
         plyr.Points += ptsForPATcatches;
#if DEBUG
         Utility.Announce( string.Format(
            "{0} has {1} points for PAT catches", plyr.PlayerName, ptsForPATcatches ) );
#endif

         //  1 pt / 10 yds
         var ptsForYDs = PointsForStats(
            plyr: plyr,
            increment: 1,
            forStatType: Constants.K_STATCODE_RECEPTION_YARDS,
            divisor: 10.0M );

         plyr.Points += ptsForYDs;
#if DEBUG
         Utility.Announce( string.Format(
            "{0} has {1} points for YDc", plyr.PlayerName, ptsForYDs ) );
#endif

         #endregion Catching

         #region Running

         //  6 points for TD run
         var ptsForTDruns = PointsFor( plyr, 6, Constants.K_SCORE_TD_RUN, id: "1" );
         plyr.Points += ptsForTDruns;
#if DEBUG
         Utility.Announce( string.Format(
            "{0} has {1} points for TD runs", plyr.PlayerName, ptsForTDruns ) );
#endif

         var ptsForPaTruns = PointsFor( plyr, 2, Constants.K_SCORE_PAT_RUN, id: "1" );
         plyr.Points += ptsForPaTruns;
#if DEBUG
         Utility.Announce( string.Format(
            "{0} has {1} points for PAT runs", plyr.PlayerName, ptsForPaTruns ) );
#endif

         //  1 pt / 10 yds
         var ptsForYDr = PointsForStats(
            plyr: plyr, increment: 1, forStatType: Constants.K_STATCODE_RUSHING_YARDS, divisor: 10.0M );
         plyr.Points += ptsForYDr;
#if DEBUG
         Utility.Announce( string.Format(
            "{0} has {1} points for YDr", plyr.PlayerName, ptsForYDr ) );
#endif

         #endregion Running

         #region Kicking

         plyr.Points += PointsFor( plyr, 3, Constants.K_SCORE_FIELD_GOAL, "1" );

         plyr.Points += PointsFor( plyr, 1, Constants.K_SCORE_PAT, "1" );

         #endregion Kicking

#if DEBUG
         Utility.Announce( string.Format(
            "{0} has {1} in week {2}:{3}",
               plyr.PlayerName, plyr.Points, week.Season, week.Week ) );
#endif
         // side effect - store metrics
         if (!WeekHasPassed) return plyr.Points;

         if ( plyr.GameMetrics.ContainsKey( GameKey ) )
            PgmDao.SaveActuals( plyr.GameMetrics[ GameKey ]  );
         else
         {
#if DEBUG
            Utility.Announce( string.Format( "{0} not found in player Game Metrics", GameKey ) );
#endif               
         }
         return plyr.Points;
      }

      public string Name { get; set; }

      public XmlCache Master
      {
         get { throw new NotImplementedException(); }
         set { throw new NotImplementedException(); }
      }

      public NFLWeek Week { get; set; }

      #endregion IRatePlayers Members

      private int PointsForStats( NFLPlayer plyr, int increment, string forStatType, decimal divisor )
      {
         var qty = 0.0M;
         if ( WeekHasPassed )
         {
            var ds = plyr.LastStats( forStatType, Week.WeekNo, Week.WeekNo, Week.Season );
            foreach ( DataRow dr in ds.Tables[ 0 ].Rows )
               qty = Decimal.Parse( dr[ "QTY" ].ToString() );

            switch ( forStatType )
            {
               case Constants.K_STATCODE_PASSING_YARDS:
                  plyr.ProjectedYDp = Convert.ToInt32( qty );
                  break;

               case Constants.K_STATCODE_PASSES_CAUGHT:
                  plyr.ProjectedReceptions = Convert.ToInt32( qty );
                  break;

               case Constants.K_STATCODE_RUSHING_YARDS:
                  plyr.ProjectedYDr = Convert.ToInt32( qty );
                  break;

               case Constants.K_STATCODE_INTERCEPTIONS_THROWN:
                  break;

               case Constants.K_STATCODE_RECEPTION_YARDS:
                  break;

               default:
                  Utility.Announce( string.Format( "Unknown stat type {0}", forStatType ) );
                  break;
            }
            plyr.AddMetric( forStatType, GameKey, qty, 0 );
         }
         else
         {
            //  game not played yet
            if ( !string.IsNullOrEmpty( GameKey ) )
            {
               var pgm = PgmDao.GetPlayerWeek( GameKey, plyr.PlayerCode );
               qty = PgmDao.ProjectedStatsOfType( forStatType, pgm );
            }
         }

         var pts = Math.Floor( qty / divisor );
         var points = Convert.ToInt32( pts ) * increment;

         return points;
      }

      private decimal PointsFor(
         NFLPlayer plyr, int increment,
         string forScoreType, string id )
      {
         decimal nScores;
         var ds = new DataSet();
         if ( WeekHasPassed )
         {
            ds = plyr.LastScores(
               forScoreType, Week.WeekNo, Week.WeekNo, Week.Season, id );
            nScores = ds.Tables[ 0 ].Rows.Count;
	         if (forScoreType == Constants.K_SCORE_TD_PASS && id == "1")
		         forScoreType = Constants.K_SCORE_TD_CATCH;
            plyr.AddMetric( forScoreType, GameKey, nScores );
         }
         else
         {
            //  use projections
            var dao = new DbfPlayerGameMetricsDao();
            var pgm = dao.GetPlayerWeek( GameKey, plyr.PlayerCode );
            nScores = pgm.ProjectedScoresOfType( forScoreType, id );
         }
         var points = nScores * increment;

         switch ( forScoreType )
         {
            case Constants.K_SCORE_TD_PASS:
               if ( plyr.PlayerCat.Equals( Constants.K_QUARTERBACK_CAT ) && ( id == "2" ) )
                  plyr.ProjectedTDp = nScores;
               else
                  plyr.ProjectedTDc = nScores;
               break;

            case Constants.K_SCORE_TD_RUN:
               plyr.ProjectedTDr = nScores;
               break;

            case Constants.K_SCORE_FIELD_GOAL:
               plyr.ProjectedFg = (int) nScores;
               if ( WeekHasPassed )
                  points = ScoreFG( table: ds.Tables[ 0 ] );
               break;

            case Constants.K_SCORE_PAT_PASS:
               break;

            case Constants.K_SCORE_PAT:
               break;

            case Constants.K_SCORE_PAT_RUN:
               break;

				case Constants.K_SCORE_TD_CATCH:
					plyr.ProjectedTDc = nScores;
		         break;

            default:
               Utility.Announce( string.Format(
                  "YahooScorer: Unknown score type {0}", forScoreType ) );
               break;
         }
         return points;
      }

      private static int ScoreFG( DataTable table )
      {
         var pts = 0;
         foreach ( DataRow dr in table.Rows )
         {
            if ( Int32.Parse( dr[ "DISTANCE" ].ToString() ) > 49 )
               pts += 5;
            else if ( Int32.Parse( dr[ "DISTANCE" ].ToString() ) > 39 )
               pts += 4;
            else
               pts += 3;
         }
         return pts;
      }
   }
}