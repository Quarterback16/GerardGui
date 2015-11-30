using System;
using System.Collections.Generic;
using System.Data;

namespace RosterLib
{
   public class DbfPlayerGameMetricsDao : IPlayerGameMetricsDao
   {
      public IPgmMaster MyPgmMaster { get; set; }

      public DbfPlayerGameMetricsDao()
      {
         MyPgmMaster = new PgmMaster();
      }

      public PlayerGameMetrics Get(string playerCode, string gameCode)
      {
         //  the caching idea doesnt work very well as it actually creates duplicate records
         //  in the database esp when testing

         PlayerGameMetrics pgm;

         //var pgm = MyPgmMaster.GetPgm(playerCode, gameCode);

         //if (string.IsNullOrEmpty(pgm.GameKey))
         //{
         //   pgm.GameKey = gameCode;
         //   pgm.PlayerId = playerCode;
         //   pgm.IsEmpty = true;
         //   return pgm;  //  send back a record for possible population
         //}

         var ds = Utility.TflWs.GetPlayerGameMetrics(playerCode, gameCode);

         if (ds.Tables[0].Rows.Count > 0)
         {
            pgm = new PlayerGameMetrics
            {
               PlayerId = playerCode,
               GameKey = gameCode,
               ProjTDp = IntValue( ds, "projtdp" ),
               ProjTDr = IntValue( ds, "projtdr" ),
               ProjTDc = IntValue( ds, "projtdc" ),
               ProjYDp = IntValue( ds, "projydp" ),
               ProjYDr = IntValue( ds, "projydr" ),
               ProjYDc = IntValue( ds, "projydc" ),
               ProjFG  = IntValue( ds, "projfg" ),
               ProjPat = IntValue( ds, "projpat" ),
               TDp = IntValue(ds, "tdp"),
               TDr = IntValue(ds, "tdr"),
               TDc = IntValue(ds, "tdc"),
               YDp = IntValue(ds, "ydp"),
               YDr = IntValue(ds, "ydr"),
               YDc = IntValue(ds, "ydc"),
               FG  = IntValue(ds, "fg"),
               Pat = IntValue(ds, "pat"),
               IsEmpty = false
            };
         }
         else
         {
            pgm = new PlayerGameMetrics();
            pgm.PlayerId = playerCode;
            pgm.GameKey = gameCode;
            pgm.ProjTDp = 0;
            pgm.ProjTDr = 0;
            pgm.ProjTDc = 0;
            pgm.ProjYDp = 0;
            pgm.ProjYDr = 0;
            pgm.ProjYDc = 0;
            pgm.ProjFG = 0;
            pgm.ProjPat = 0;
         }
         MyPgmMaster.PutPgm(pgm);
         return pgm;
      }

      private static int IntValue( DataSet ds, string fieldName)
      {
         return Int32.Parse(ds.Tables[0].Rows[0][fieldName].ToString());
      }

      public void Save(PlayerGameMetrics pgm)
      {
         string commandStr;
         var oldPgm = Get(pgm.PlayerId, pgm.GameKey);
         if (oldPgm.IsEmpty)
         {
            Utility.TflWs.InsertPlayerGameMetric(
                     pgm.PlayerId, pgm.GameKey,
                     pgm.ProjYDp, pgm.YDp, pgm.ProjYDr, pgm.YDr,
                     pgm.ProjTDp, pgm.TDp, pgm.ProjTDr, pgm.TDr, pgm.ProjTDc, pgm.TDc, pgm.ProjYDc, pgm.YDc,
                     pgm.ProjFG, pgm.FG, pgm.ProjPat, pgm.Pat
               );
            commandStr = "Player Metric not found";
         }
         else
         {
            commandStr =
            Utility.TflWs.UpdatePlayerGameMetric(
                     pgm.PlayerId, pgm.GameKey,
                     pgm.ProjYDp, pgm.YDp, pgm.ProjYDr, pgm.YDr,
                     pgm.ProjTDp, pgm.TDp, pgm.ProjTDr, pgm.TDr, pgm.ProjTDc, pgm.TDc, pgm.ProjYDc, pgm.YDc,
                     pgm.ProjFG, pgm.FG, pgm.ProjPat, pgm.Pat
               );
#if DEBUG
            Utility.Announce(string.Format("Command is {0} on {1}", commandStr, Utility.TflWs.NflConnectionString));
#endif
         }
      }

	   public void SaveActuals(PlayerGameMetrics pgm)
	   {
         string commandStr;
         var oldPgm = Get(pgm.PlayerId, pgm.GameKey);
		   if (oldPgm.IsEmpty)
		   {
			   Utility.TflWs.InsertPlayerGameMetric(
				   pgm.PlayerId, pgm.GameKey,
				   pgm.ProjYDp, pgm.YDp, pgm.ProjYDr, pgm.YDr,
				   pgm.ProjTDp, pgm.TDp, pgm.ProjTDr, pgm.TDr, pgm.ProjTDc, pgm.TDc, pgm.ProjYDc, pgm.YDc,
				   pgm.ProjFG, pgm.FG, pgm.ProjPat, pgm.Pat
				   );
			   commandStr = "Player Metric not found";
		   }
		   else
		   {
			   commandStr =
				   Utility.TflWs.UpdatePlayerGameMetricWithActuals(
					   pgm.PlayerId, pgm.GameKey,
					   pgm.YDp, pgm.YDr,
					   pgm.TDp, pgm.TDr, pgm.TDc, pgm.YDc,
					   pgm.FG, pgm.Pat,
						pgm.FantasyPoints
					   );
		   }
#if DEBUG
         Utility.Announce(string.Format("Command is {0} on {1}", commandStr, Utility.TflWs.NflConnectionString));
#endif
	   }

	   public List<PlayerGameMetrics> GetWeek(string season, string week)
      {
         var pgmList = new List<PlayerGameMetrics>();
         DataSet ds = Utility.TflWs.GetAllPlayerGameMetrics(season, week);
         DataTable dt = ds.Tables[0];
         foreach (DataRow dr in dt.Rows)
         {
            var pgm = new PlayerGameMetrics();
            pgm.PlayerId = dr["PLAYERID"].ToString();
            pgm.GameKey = dr["GAMECODE"].ToString();
            pgm.ProjTDp = IntValue(dr, "projtdp");
            pgm.ProjTDr = IntValue(dr, "projtdr");
            pgm.ProjTDc = IntValue(dr, "projtdc");
            pgm.ProjYDp = IntValue(dr, "projydp");
            pgm.ProjYDr = IntValue(dr, "projydr");
            pgm.ProjYDc = IntValue(dr, "projydc");
            pgm.ProjFG = IntValue(dr, "projfg");
            pgm.ProjPat = IntValue(dr, "projpat");
            pgmList.Add(pgm);
         }
#if DEBUG2
         Utility.Announce(string.Format("Metric records loaded : {0}", pgmList.Count));
#endif
         return pgmList;
      }

      public PlayerGameMetrics GetPlayerWeek(string gameCode, string playerId )
      {
         var pgm = new PlayerGameMetrics();
         var ds = Utility.TflWs.GetPlayerGameMetrics( playerId, gameCode);
         var dt = ds.Tables[0];
         foreach (DataRow dr in dt.Rows)
         {
            pgm.PlayerId = dr["PLAYERID"].ToString();
            pgm.GameKey = dr["GAMECODE"].ToString();
            pgm.ProjTDp = IntValue(dr, "projtdp");
            pgm.ProjTDr = IntValue(dr, "projtdr");
            pgm.ProjTDc = IntValue(dr, "projtdc");
            pgm.ProjYDp = IntValue(dr, "projydp");
            pgm.ProjYDr = IntValue(dr, "projydr");
            pgm.ProjYDc = IntValue(dr, "projydc");
            pgm.ProjFG  = IntValue(dr, "projfg");
            pgm.ProjPat = IntValue(dr, "projpat");
            pgm.TDp = IntValue( dr, "tdp" );
            pgm.TDr = IntValue( dr, "tdr" );
            pgm.TDc = IntValue( dr, "tdc" );
            pgm.YDp = IntValue( dr, "ydp" );
            pgm.YDr = IntValue( dr, "ydr" );
            pgm.YDc = IntValue( dr, "ydc" );
            pgm.FG  = IntValue( dr, "fg" );
            pgm.Pat = IntValue( dr, "pat" );
            break;
         }
         return pgm;
      }

      private static int IntValue( DataRow dr, string fieldName)
      {
         return Int32.Parse(dr[fieldName].ToString());
      }

      public void ClearGame(string gameKey)
      {
         Utility.TflWs.ClearPlayerGameMetrics(gameKey);
      }

      public List<PlayerGameMetrics> GetSeason(string season, string playerCode)
      {
         var pgmList = new List<PlayerGameMetrics>();
         var ds = Utility.TflWs.GetAllPlayerGameMetricsForPlayer(season, playerCode );
         var dt = ds.Tables[0];
         foreach (DataRow dr in dt.Rows)
         {
            var pgm = new PlayerGameMetrics
               {
                  PlayerId = dr[ "PLAYERID" ].ToString(),
                  GameKey = dr[ "GAMECODE" ].ToString(),
                  ProjTDp = IntValue( dr, "projtdp" ),
                  ProjTDr = IntValue( dr, "projtdr" ),
                  ProjTDc = IntValue( dr, "projtdc" ),
                  ProjYDp = IntValue( dr, "projydp" ),
                  ProjYDr = IntValue( dr, "projydr" ),
                  ProjYDc = IntValue( dr, "projydc" ),
                  ProjFG = IntValue( dr, "projfg" ),
                  ProjPat = IntValue( dr, "projpat" )
               };
            pgmList.Add(pgm);
         }
#if DEBUG2
         Utility.Announce(string.Format("Metric records loaded : {0}", pgmList.Count));
#endif

         return pgmList;
      }

      public decimal ProjectedStatsOfType( string forStatType, PlayerGameMetrics pgm )
      {
         var qty = 0.0M;
         switch (forStatType)
         {
            case Constants.K_STATCODE_RECEPTION_YARDS:
               qty = pgm.ProjYDc;
               break;
            case Constants.K_STATCODE_RUSHING_YARDS:
               qty = pgm.ProjYDr;
               break;
            case Constants.K_STATCODE_PASSING_YARDS:
               qty = pgm.ProjYDp;
               break;
         }
         return qty;
      }
   }
}