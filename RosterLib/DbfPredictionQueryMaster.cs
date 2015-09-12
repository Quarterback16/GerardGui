using System;

namespace RosterLib
{
	public class DbfPredictionQueryMaster : IGetPredictions
	{
		public Prediction Get( string method, string season, string week, string gameCode )
		{
			var prediction = new Prediction();

			var ds = Utility.TflWs.GetPrediction(method, season, week, gameCode);

			if ( ds.Tables[ 0 ].Rows.Count == 1 )
			{
				var game = new NFLGame( string.Format( "{0}:{1}-{2}", season, week, gameCode ) );
				prediction.Method = method;
				prediction.Season = season;
				prediction.Week = week;
				prediction.GameCode = gameCode;
				prediction.HomeScore = IntValue( ds, "HomeScore" );
				prediction.AwayScore = IntValue( ds, "AwayScore" );
				//  also do a result
				var result = new NFLResult( home: game.HomeTeam, homePts: prediction.HomeScore,
					away: game.AwayTeam, awayPts: prediction.AwayScore );
				result.HomeTDp = IntValue( ds, "htdp" );
				result.HomeTDr = IntValue( ds, "htdr" );
				result.HomeTDd = IntValue( ds, "htdd" );
				result.HomeTDs = IntValue( ds, "htds" );
				result.AwayTDp = IntValue( ds, "atdp" );
				result.AwayTDr = IntValue( ds, "atdr" );
				result.AwayTDd = IntValue( ds, "atdd" );
				result.AwayTDs = IntValue( ds, "atds" );
				result.HomeYDp = IntValue( ds, "hydp" );
				result.AwayYDp = IntValue( ds, "aydp" );
				result.HomeYDr = IntValue( ds, "hydr" );
				result.AwayYDr = IntValue( ds, "aydr" );
				result.AwayFg = IntValue(ds, "afg");
				result.HomeFg = IntValue(ds, "hfg");
				prediction.NflResult = result;
			}
			return prediction;
		}

		private static int IntValue( System.Data.DataSet ds, string fieldName )
		{
			return Int32.Parse( ds.Tables[ 0 ].Rows[ 0 ][ fieldName ].ToString() );
		}
	}
}
