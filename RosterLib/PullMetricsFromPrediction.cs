using NLog;
using System;
using System.Collections.Generic;

namespace RosterLib
{
	// filter
	public class PullMetricsFromPrediction
	{
		public Logger Logger { get; set; }

		public PullMetricsFromPrediction( PlayerGameProjectionMessage input )
		{
			Logger = LogManager.GetCurrentClassLogger();
			Process( input );
		}

		private void Process( PlayerGameProjectionMessage input )
		{
			if ( input.Game == null ) return;

			Logger.Trace( string.Format( "Processing {0}:{1}", input.Game.GameCodeOut(), input.Game.GameName() ) );
			DoRushingUnit( input, input.Game.HomeNflTeam.TeamCode, isHome: true );
			DoRushingUnit( input, input.Game.AwayNflTeam.TeamCode, isHome: false );
			DoPassingUnit( input, input.Game.HomeNflTeam.TeamCode, isHome: true );
			DoPassingUnit( input, input.Game.AwayNflTeam.TeamCode, isHome: false );
			DoKickingUnit( input, input.Game.HomeNflTeam.TeamCode, isHome: true );
			DoKickingUnit( input, input.Game.AwayNflTeam.TeamCode, isHome: false );
			if ( input.Game.PlayerGameMetrics.Count < 12 )
				Utility.Announce( string.Format( "Missing metrics from {0}", input.Game ) );
		}

		#region Kicking

		private void DoKickingUnit( PlayerGameProjectionMessage input, string teamCode, bool isHome )
		{
			var kicker = GetKicker( teamCode );
			if ( kicker != null )
			{
				var projFG = ( isHome ) ? input.Prediction.HomeFg : input.Prediction.AwayFg;
				var projPat = ( isHome ) ? input.Prediction.TotalHomeTDs() : input.Prediction.TotalAwayTDs();
				AddKickingPlayerGameMetric( input, kicker.PlayerCode, projFG, projPat );
			}
			else
				Utility.Announce( string.Format( "No kicker found for {0}", teamCode ) );
		}

		private static NFLPlayer GetKicker( string teamCode )
		{
			var team = new NflTeam( teamCode );
			team.SetKicker();
			return team.Kicker;
		}

		private static void AddKickingPlayerGameMetric( PlayerGameProjectionMessage input,
						string playerId, int projFg, int projPat )
		{
			if ( input == null || playerId == null ) return;
			var pgm = new PlayerGameMetrics
			{
				PlayerId = playerId,
				GameKey = input.Game.GameKey(),
				ProjFG = projFg,
				ProjPat = projPat
			};
			input.Game.PlayerGameMetrics.Add( pgm );
		}

		#endregion Kicking

		#region Passing

		private void DoPassingUnit( PlayerGameProjectionMessage input, string teamCode, bool isHome )
		{
			var unit = new PassUnit();
			unit.Load( teamCode );

			// give it to the QB
			if ( unit.Q1 != null )
			{
				var projYDp = ( isHome ) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp;
				var projTDp = ( isHome ) ? input.Prediction.HomeTDp : input.Prediction.AwayTDp;
				AddPassinglayerGameMetric( input, unit.Q1.PlayerCode, projYDp, projTDp );
			}
			// Receivers
			int projYDc, projTDc;
			if ( unit.W1 != null )
			{
				projYDc = ( int ) ( .4 * ( ( isHome ) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp ) );
				projTDc = W1TdsFrom( ( isHome ) ? input.Prediction.HomeTDp : input.Prediction.AwayTDp );
				projYDc = AllowForInjuryRisk( unit.W1, projYDc );
				AddCatchingPlayerGameMetric( input, unit.W1.PlayerCode, projYDc, projTDc );
			}
			if ( unit.W2 != null )
			{
				projYDc = ( int ) ( .25 * ( ( isHome ) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp ) );
				projTDc = W2TdsFrom( ( isHome ) ? input.Prediction.HomeTDp : input.Prediction.AwayTDp );
				projYDc = AllowForInjuryRisk( unit.W2, projYDc );
				AddCatchingPlayerGameMetric( input, unit.W2.PlayerCode, projYDc, projTDc );
			}
			if ( unit.W3 != null )
			{
				projYDc = ( int ) ( .1 * ( ( isHome ) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp ) );
				projTDc = 0;
				projYDc = AllowForInjuryRisk( unit.W3, projYDc );
				AddCatchingPlayerGameMetric( input, unit.W3.PlayerCode, projYDc, projTDc );
			}
			if ( unit.TE != null )
			{
				projYDc = ( int ) ( .25 * ( ( isHome ) ? input.Prediction.HomeYDp : input.Prediction.AwayYDp ) );
				projTDc = TETdsFrom( ( isHome ) ? input.Prediction.HomeTDp : input.Prediction.AwayTDp );
				projYDc = AllowForInjuryRisk( unit.TE, projYDc );
				AddCatchingPlayerGameMetric( input, unit.TE.PlayerCode, projYDc, projTDc );
			}
		}

		private void AddCatchingPlayerGameMetric( PlayerGameProjectionMessage input,
		   string playerId, int projYDc, int projTDc )
		{
			if ( input == null || playerId == null ) return;
			if ( string.IsNullOrEmpty( playerId ) || input.Game == null ) return;
			var pgm = new PlayerGameMetrics
			{
				PlayerId = playerId,
				GameKey = input.Game.GameKey(),
				ProjYDc = projYDc,
				ProjTDc = projTDc
			};
#if DEBUG
			Utility.Announce( pgm.ToString() );
#endif
			input.Game.PlayerGameMetrics.Add( pgm );
		}

		private static int W1TdsFrom( int totalTds )
		{
			var tds = 0;
			switch ( totalTds )
			{
				case 1:
					tds = 1;
					break;

				case 2:
					tds = 1;
					break;

				case 3:
					tds = 1;
					break;

				case 4:
					tds = 2;
					break;

				case 5:
					tds = 2;
					break;

				case 6:
					tds = 3;
					break;
			}
			return tds;
		}

		private static int W2TdsFrom( int totalTds )
		{
			var tds = 0;
			switch ( totalTds )
			{
				case 1:
					tds = 0;
					break;

				case 2:
					tds = 1;
					break;

				case 3:
					tds = 1;
					break;

				case 4:
					tds = 1;
					break;

				case 5:
					tds = 2;
					break;

				case 6:
					tds = 2;
					break;
			}
			return tds;
		}

		private static int TETdsFrom( int totalTds )
		{
			var tds = 0;
			switch ( totalTds )
			{
				case 1:
					tds = 0;
					break;

				case 2:
					tds = 0;
					break;

				case 3:
					tds = 1;
					break;

				case 4:
					tds = 1;
					break;

				case 5:
					tds = 1;
					break;

				case 6:
					tds = 1;
					break;
			}
			return tds;
		}

		private static void AddPassinglayerGameMetric( PlayerGameProjectionMessage input, string playerId, int projYDp, int projTDp )
		{
			if ( input == null || playerId == null ) return;
			var pgm = new PlayerGameMetrics
			{
				PlayerId = playerId,
				GameKey = input.Game.GameKey(),
				ProjYDp = projYDp,
				ProjTDp = projTDp
			};
#if DEBUG
			Utility.Announce( pgm.ToString() );
#endif
			input.Game.PlayerGameMetrics.Add( pgm );
		}

		#endregion Passing

		#region Rushing

		private static void DoRushingUnit(
			PlayerGameProjectionMessage input, string teamCode, bool isHome )
		{
			var ru = new RushUnit();
			ru.Load( teamCode );

			if ( ru.IsAceBack )
			{
				//  R1
				var percentageOfAction = 0.7M;
				if ( ru.R2 == null ) percentageOfAction = 0.9M;
				var projYDr = ( int ) ( percentageOfAction * ( ( isHome ) ? input.Prediction.HomeYDr : input.Prediction.AwayYDr ) );
				//  Injury penalty
				projYDr = AllowForInjuryRisk( ru.AceBack, projYDr );
				var projTDrAce = R1TdsFrom( ( isHome ) ? input.Prediction.HomeTDr : input.Prediction.AwayTDr );
				var isVulture = AllowForVulturing( ru.AceBack.PlayerCode, ref projTDrAce, ru );
				AddPlayerGameMetric( input, ru.AceBack.PlayerCode, projYDr, projTDrAce );
				//  R2 optional
				if ( ru.R2 != null )
				{
					projYDr = ( int ) ( .2 * ( ( isHome ) ? input.Prediction.HomeYDr : input.Prediction.AwayYDr ) );
					projYDr = AllowForInjuryRisk( ru.AceBack, projYDr );
					var projTDr = R2TdsFrom( ( isHome ) ? input.Prediction.HomeTDr : input.Prediction.AwayTDr );
					if ( isVulture )
						projTDr = AllowForR2BeingTheVulture( projTDr, ru.R2.PlayerCode, ru );
					AddPlayerGameMetric( input, ru.R2.PlayerCode, projYDr, projTDr );
				}
			}
			else
			{
				//  Comittee
				const decimal percentageOfAction = 0.5M;
				foreach ( var runner in ru.Starters )
				{
					var projYDr = ( int ) ( percentageOfAction * ( ( isHome ) ? input.Prediction.HomeYDr : input.Prediction.AwayYDr ) );
					projYDr = AllowForInjuryRisk( runner, projYDr );
					var projTDr = 0M;
					var tds = 0;
					if ( isHome )
					{
						tds = R1TdsFrom( input.Prediction.HomeTDr );
						projTDr = decimal.Divide( ( decimal ) tds, 2M );
					}
					else
					{
						tds = R1TdsFrom( input.Prediction.AwayTDr );
						projTDr = decimal.Divide( ( decimal ) tds, 2M );
					}

					AddPlayerGameMetric( input, runner.PlayerCode, projYDr, projTDr );
				}
			}
		}

		private static int AllowForR2BeingTheVulture( int projTDr, string r2Id, RushUnit ru )
		{
			if ( r2Id == ru.GoalLineBack.PlayerCode )
				projTDr++;
			return projTDr;
		}

		private static bool AllowForVulturing( string runnerId, ref int projTDr, RushUnit ru )
		{
			var isVulture = false;

			if ( projTDr > 0 )
			{
				if ( ru.GoalLineBack != null )
				{
					if ( ru.GoalLineBack.PlayerCode != runnerId )
					{
						//  Vulture
						projTDr--;
						isVulture = true;
					}
				}
			}
			return isVulture;
		}

		private static int R1TdsFrom( int totalTdr )
		{
			var tdrs = 0;
			switch ( totalTdr )
			{
				case 1:
					tdrs = 1;
					break;

				case 2:
					tdrs = 1;
					break;

				case 3:
					tdrs = 2;
					break;

				case 4:
					tdrs = 3;
					break;

				case 5:
					tdrs = 3;
					break;

				case 6:
					tdrs = 3;
					break;
			}
			return tdrs;
		}

		private static int R2TdsFrom( int totalTdr )
		{
			var tdrs = 0;
			switch ( totalTdr )
			{
				case 1:
					tdrs = 0;
					break;

				case 2:
					tdrs = 1;
					break;

				case 3:
					tdrs = 1;
					break;

				case 4:
					tdrs = 1;
					break;

				case 5:
					tdrs = 2;
					break;

				case 6:
					tdrs = 2;
					break;
			}
			return tdrs;
		}

		#endregion Rushing

		private static void AddPlayerGameMetric( PlayerGameProjectionMessage input, string playerId, int projYDr, decimal projTDr )
		{
			var pgm = new PlayerGameMetrics
			{
				PlayerId = playerId,
				GameKey = input.Game.GameKey(),
				ProjYDr = projYDr,
				ProjTDr = projTDr
			};
#if DEBUG
			Utility.Announce( pgm.ToString() );
#endif
			if ( input.Game.PlayerGameMetrics == null ) input.Game.PlayerGameMetrics = new List<PlayerGameMetrics>();
			input.Game.PlayerGameMetrics.Add( pgm );
		}

		public static int AllowForInjuryRisk( NFLPlayer p, int proj )
		{
			if ( p == null )
				Utility.Announce( "AllowForInjuryRisk:Null Player" );
			else
			{
				Int32.TryParse( p.Injuries(), out int nInjury );
				if ( nInjury > 0 )
				{
					var injChance = ( ( nInjury * 10.0M ) / 100.0M );
					var effectiveness = 1 - injChance;
					proj = ( int ) ( proj * effectiveness );
				}
			}
			return proj;
		}
	}
}