namespace RosterLib
{
	public class DbfPredictionStorer : IStorePredictions
	{
		public void StorePrediction(string method, NFLGame game, NFLResult result)
		{
			//  Validate prediction 
			if ( !result.IsValid() )
				Utility.Announce( "Invalid prediction - " + game.GameName() );

			//  See if we have this prediction already
			var ds = Utility.TflWs.GetPrediction(method, game.Season, game.Week, game.GameCode);

			if (ds.Tables[0].Rows.Count == 1)
				//  if yes just update
				Utility.TflWs.UpdatePrediction(
					method, game.Season, game.Week, game.GameCode, result.HomeScore, result.AwayScore,
					result.HomeTDp, result.HomeTDr, result.HomeTDd, result.HomeTDs, result.HomeFg,
					result.AwayTDp, result.AwayTDr, result.AwayTDd, result.AwayTDs, result.AwayFg,
 					result.HomeYDp, result.HomeYDr, result.AwayYDp, result.AwayYDr 
					);
			else
				//  otherwise insert
				Utility.TflWs.InsertPrediction(
					method, game.Season, game.Week, game.GameCode, result.HomeScore, result.AwayScore,
					result.HomeTDp, result.HomeTDr, result.HomeTDd, result.HomeTDs, result.HomeFg, 
					result.AwayTDp, result.AwayTDr, result.AwayTDd, result.AwayTDs, result.AwayFg,
 					result.HomeYDp, result.HomeYDr, result.AwayYDp, result.AwayYDr 
			      );

			//TODO:  this stuffs up the scores if using the Miller predictions
			//// also update Game projections (used by Starters())
			//Utility.TflWs.StoreResult(game.Season, game.Week, game.GameCode, result.AwayScore, result.HomeScore, result.HomeTDp,
			//                          result.AwayTDp, result.HomeTDr, result.AwayTDr, result.HomeFg, result.AwayFg,
			//                          result.AwayTDd, result.HomeTDd, result.AwayTDs, result.HomeTDs);
		}
	}
}