﻿using System;
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
         // build the winners list
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

      private int GenerateChart( StringBuilder bodyOut, YahooCalculator c, int lineNo, IWinOrLose team )
      {
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
         bodyOut.AppendLine( string.Format( " {0}", te ) );
         return lineNo;
      }

      #region  Bits and Pieces

      private static string GameBit( IWinOrLose team )
      {
         team.Game.CalculateSpreadResult();
         var predictedResult = team.IsWinner ? team.Game.BookieTip.PredictedScore() : team.Game.BookieTip.PredictedScoreFlipped();
         var theLine = team.Game.TheLine();
	      var url = team.Game.GameProjectionUrl();
         return string.Format( "<a href='{0}'>{1}</a> {2,3}", url, predictedResult, theLine );
      }

      private static string GetW1Bit( IWinOrLose team, YahooCalculator c )
      {
         var bit = "none";
         if ( team.Team.PassUnit.W1 != null )
         {
            bit = PlayerPiece( team.Team.PassUnit.W1, team.Game, c );
         }
         return string.Format( "{0,-36}", bit );
      }

      private static string GetW2Bit( IWinOrLose team, YahooCalculator c )
      {
         var bit = "none";
         if ( team.Team.PassUnit.W2 != null )
         {
            bit = PlayerPiece( team.Team.PassUnit.W2, team.Game, c );
         }
         return string.Format( "{0,-36}", bit );
      }

      private string GetTEBit( IWinOrLose team, YahooCalculator c )
      {
         var bit = "none";
         if ( team.Team.PassUnit.TE != null )
         {
            bit = PlayerPiece( team.Team.PassUnit.TE, team.Game, c );
         }
         return string.Format( "{0,-36}", bit );
      }

      private static string GetQBBit( IWinOrLose team, YahooCalculator c )
      {
         var bit = "none";
         if (team.Team.PassUnit.Q1 != null)
            bit = PlayerPiece( team.Team.PassUnit.Q1, team.Game, c );
         return string.Format( "{0,-36}", bit );
      }

      private static string GetRunnerBit( IWinOrLose team, YahooCalculator c )
      {
         var bit = "dual";
         if (team.Team.RushUnit.AceBack != null)
            bit = PlayerPiece( team.Team.RushUnit.AceBack, team.Game, c );
         return string.Format( "{0,-36}", bit );
      }

      private static string PlayerPiece( NFLPlayer p, NFLGame g, YahooCalculator c )
      {
         var nextOppTeam = p.NextOpponentTeam( g );
         var defensiveRating = nextOppTeam.DefensiveRating( p.PlayerCat );
         var owners = p.LoadAllOwners();
         c.Calculate( p, g );
	      var namePart = string.Format( "<a href='{0}.htm'>{1}</a>", p.PlayerCode, p.PlayerNameTo( 11 ) );
         return string.Format( "{0,-11} {3}  {1}  {2,2:#0}   ____", namePart, defensiveRating, p.Points, owners );
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