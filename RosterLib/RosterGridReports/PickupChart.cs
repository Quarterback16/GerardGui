using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RosterLib.RosterGridReports
{
   public class PickupChart : RosterGridReport
   {
      public int Week { get; set; }

      public SimplePreReport Report { get; set; }

      public PickupChart( string season, int week )
      {
         Name = "Pickup Chart";
         Season = season;
         Week = week;
         Report = new SimplePreReport
         {
            ReportType = "Pickup Chart",
            Folder = "Projections",
            Season = Season,
            InstanceName = string.Format( "Pickup-Chart-Week-{0:0#}", Week )
         };
      }

      public override void RenderAsHtml()
      {
         Report.Body = GenerateBody();
         Report.RenderHtml();
         FileOut = Report.FileOut;
      }

      private string GenerateBody()
      {
         var bodyOut = new StringBuilder();

         var winners = GetWinners();
         var losers = GetLosers();

         var winnersList = winners.ToList().OrderByDescending( x => x.Margin );
         var losersList = losers.ToList().OrderBy( x => x.Margin );

         var c = new YahooCalculator();
         var lineNo = 0;

         foreach ( var winner in winnersList )
            lineNo = GenerateChart( bodyOut, c, lineNo, winner );

         foreach ( var loser in losersList )
            lineNo = GenerateChart( bodyOut, c, lineNo, loser );

         return bodyOut.ToString();
      }

      private int GenerateChart( 
         StringBuilder bodyOut, YahooCalculator c, int lineNo, IWinOrLose team )
      {
	      team.Team.LoadKickUnit();
         team.Team.LoadRushUnit();
         team.Team.LoadPassUnit();
         var qb = GetQBBit( team, c );
         var rb = GetRunnerBit( team, c );
         var gameBit = GameBit( team );

         lineNo++;
         bodyOut.Append( string.Format( "{0,2}  {1}", lineNo, gameBit ) );
         bodyOut.Append( string.Format( " {0}", qb ) );
         bodyOut.Append( string.Format( " {0}", rb ) );
         //    spit out the WR1 line
         var wr1 = GetW1Bit( team, c );
         bodyOut.Append( string.Format( " {0}", wr1 ) );
         //    spit out the WR2 line
         var wr2 = GetW2Bit( team, c );
         bodyOut.Append( string.Format( " {0}", wr2 ) );
         //    spit out the TE line
         var te = GetTEBit( team, c );
         bodyOut.Append( string.Format( " {0}", te ) );
			//    spit out the PK line
			var pk = GetPKBit(team, c);
			bodyOut.AppendLine(string.Format(" {0}", pk));

         return lineNo;
      }

      #region  Bits and Pieces

      private static string GameBit( IWinOrLose team )
      {
         team.Game.CalculateSpreadResult();
         var predictedResult = team.IsWinner 
            ? team.Game.BookieTip.PredictedScore() 
            : team.Game.BookieTip.PredictedScoreFlipped();
         var theLine = team.Game.TheLine();
	      var url = team.Game.GameProjectionUrl();
         return string.Format( "<a href='{0}'>{1}</a> {2,3}", url, predictedResult, theLine );
      }

      private string GetW1Bit( IWinOrLose team, YahooCalculator c )
      {
			var bit = NoneBit( team );

         if ( team.Team.PassUnit.W1 != null )
         {
            bit = PlayerPiece( team.Team.PassUnit.W1, team.Game, c );
         }
         return string.Format( "{0,-36}", bit );
      }

	   private string NoneBit( IWinOrLose team )
	   {
		   var bit = string.Format( "<a href='..\\Roles\\{0}-Roles-{1:0#}.htm'>none</a>                            ", team.Team.TeamCode, Week - 1 );
		   return bit;
	   }

	   private string GetW2Bit( IWinOrLose team, YahooCalculator c )
      {
			var bit = NoneBit( team );
			if ( team.Team.PassUnit.W2 != null )
         {
            bit = PlayerPiece( team.Team.PassUnit.W2, team.Game, c );
         }
         return string.Format( "{0,-36}", bit );
      }

      private string GetTEBit( IWinOrLose team, YahooCalculator c )
      {
			var bit = NoneBit( team );

         if ( team.Team.PassUnit.TE != null )
         {
            bit = PlayerPiece( team.Team.PassUnit.TE, team.Game, c );
         }
         return string.Format( "{0,-36}", bit );
      }

		private string GetPKBit(IWinOrLose team, YahooCalculator c)
		{
			var bit = NoneBit(team);

			if (team.Team.KickUnit.PlaceKicker != null)
			{
				bit = PlayerPiece(team.Team.KickUnit.PlaceKicker, team.Game, c);
			}
			return string.Format("{0,-36}", bit);
		}

      private string GetQBBit( IWinOrLose team, YahooCalculator c )
      {
			var bit = NoneBit( team );

         if (team.Team.PassUnit.Q1 != null)
            bit = PlayerPiece( team.Team.PassUnit.Q1, team.Game, c );
         return string.Format( "{0,-36}", bit );
      }

      private string GetRunnerBit( IWinOrLose team, YahooCalculator c )
      {
         var bit = NoneBit(team);
         if (team.Team.PassUnit.Q1 != null)
         {
            // get the next opponent by using the QB
            var nextOppTeam = team.Team.PassUnit.Q1.NextOpponentTeam(team.Game);
            var defensiveRating = nextOppTeam.DefensiveRating(Constants.K_RUNNINGBACK_CAT);

            bit = string.Format("<a href='..\\Roles\\{0}-Roles-{1:0#}.htm'>dual</a>                    {2}       ",
               team.Team.TeamCode, Week - 1, defensiveRating);

            if ( team.Team.RushUnit == null )
               team.Team.LoadRushUnit();
            else
               Logger.Info( "   >>> Rush unit loaded {0} rushers", team.Team.RushUnit.Runners.Count() );


            if ( team.Team.RushUnit.AceBack != null)
               bit = PlayerPiece(team.Team.RushUnit.AceBack, team.Game, c);
            else
               Logger.Info( "   >>> No Ace back for {0}", team.Team.Name );
         }
         else
         {
            Logger.Info( "   >>> No QB1 for {0}", team.Team.Name );
         }
         return string.Format( "{0,-36}", bit );
      }

      public string PlayerPiece( NFLPlayer p, NFLGame g, YahooCalculator c )
      {
         var nextOppTeam = p.NextOpponentTeam( g );
			var defensiveRating = nextOppTeam.DefensiveUnit(p.PlayerCat);
			var owners = p.LoadAllOwners();
         c.Calculate( p, g );
			var namePart = string.Format( "<a href='..\\Roles\\{0}-Roles-{1:0#}.htm'>{2}</a>", 
            p.TeamCode, Week - 1, p.PlayerNameTo( 11 ) );
         return string.Format( "{0,-11} {3}  {1}  {2,2:#0}  {4} ", 
            namePart, defensiveRating, p.Points, owners, ActualOutput(g,p) );
      }

		public string ActualOutput(NFLGame g, NFLPlayer p)
		{
			if ( ! g.Played() )
				return "____";

			Console.WriteLine(g.ScoreOut());
			if ( g.GameWeek == null ) g.GameWeek = new NFLWeek(g.Season, g.Week);
			var scorer = new YahooScorer(g.GameWeek);
			var nScore = scorer.RatePlayer(p, g.GameWeek);
			return string.Format(" {0,2:#0} ", nScore);
		}

      #endregion

      private IEnumerable<Winner> GetWinners()
      {
         var week = new NFLWeek( Season, Week );
         var winners = new List<Winner>();
         foreach ( NFLGame g in week.GameList() )
         {
            g.CalculateSpreadResult();
            var teamCode = g.BookieTip.WinningTeam();
            var winner = new Winner
            {
               Team = g.Team( teamCode ),
               Margin = Math.Abs( g.Spread ),
               Home = g.IsHome( teamCode ),
               Game = g
            };
            winners.Add( winner );
         }

         return winners;
      }

      private IEnumerable<Loser> GetLosers()
      {
         var week = new NFLWeek( Season, Week );
         var losers = new List<Loser>();
         foreach ( NFLGame g in week.GameList() )
         {
            g.CalculateSpreadResult();
            var teamCode = g.BookieTip.LosingTeam();

            var loser = new Loser
            {
               Team = g.Team( teamCode ),
               Margin = Math.Abs( g.Spread ),
               Home = g.IsHome( teamCode ),
               Game = g
            };
            losers.Add( loser );
         }

         return losers;
      }
   }

   public class Winner : IComparable, IWinOrLose
   {
      public  Winner()
      {
         IsWinner = true;
      }
      public decimal Margin { get; set; }

      public NflTeam Team { get; set; }

      public bool Home { get; set; }

      public bool IsWinner { get; set; }

      public NFLGame Game { get; set; }

      public int CompareTo( object obj )
      {
         var winner2 = ( Winner ) obj;
         return Margin > winner2.Margin ? 1 : 0;
      }

      public override string ToString()
      {
         return string.Format( "{1} by {0,4}", Margin, Team );
      }
   }

   public class Loser : IComparable, IWinOrLose
   {
      public  Loser()
      {
         IsWinner = false;
      }
      public decimal Margin { get; set; }

      public NflTeam Team { get; set; }

      public bool Home { get; set; }


      public bool IsWinner { get; set; }

      public NFLGame Game { get; set; }

      public int CompareTo( object obj )
      {
         var winner2 = ( Winner ) obj;
         return Margin < winner2.Margin ? 1 : 0;
      }

      public override string ToString()
      {
         return string.Format( "{0,4}", Margin );
      }
   }
}