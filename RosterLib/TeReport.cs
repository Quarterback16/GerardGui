using RosterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace RosterLib
{
   public class TeReport : TeamReport
   {
      public TeReport( IKeepTheTime timekeeper ) : base()
      {
         Name = "Tight End Report";
         Heading = Name;

         FileOut = string.Format( "{0}{1}//Scores//TE-Scores.htm",
                     Utility.OutputDirectory(), Season );

      }

      public override void RenderAsHtml()
      {
         Ste = DefineSte();
         Data = BuildDataTable();

         LoadDataTable();

         Render();

         Finish();
      }

      private const string FieldFormat = "Wk{0:0#}";

      private SimpleTableReport DefineSte()
      {
         var str = new SimpleTableReport( Heading );
         str.ColumnHeadings = true;
         str.DoRowNumbers = true;
         str.AddColumn( new ReportColumn( "Team", "TEAM", "{0,-20}" ) );
         str.AddColumn( new ReportColumn( "Total", "TOTAL", "{0:0.00}", typeof( decimal ), tally: true,
            colourDelegateIn: TotalBgPicker ) );

         var startAt =  Constants.K_WEEKS_IN_REGULAR_SEASON;

         for ( var w = startAt; w > 0; w-- )
         {
            var header = string.Format( "Week {0}", w );
            var fieldName = string.Format( FieldFormat, w );
            str.AddColumn( new ReportColumn( header, fieldName, "{0,5}", TeBgPicker ) );
         }
         return str;
      }

      private DataTable BuildDataTable()
      {
         var dt = new DataTable();
         var cols = dt.Columns;
         cols.Add( "TEAM", typeof( String ) );
         cols.Add( "TOTAL", typeof( decimal ) );

         for ( var w = Constants.K_WEEKS_IN_REGULAR_SEASON; w > 0; w-- )
         {
            var fieldName = string.Format( FieldFormat, w );
            cols.Add( fieldName, typeof( String ) );
         }

         dt.DefaultView.Sort = "TOTAL DESC";
         return dt;
      }

      private void LoadDataTable()
      {
         var tCount = 0;
         foreach ( KeyValuePair<string, NflTeam> teamPair in TeamList )
         {
            var team = teamPair.Value;
            DataRow teamRow = Data.NewRow();
            teamRow[ "TEAM" ] = team.NameOut();
            teamRow[ "TOTAL" ] = 0;
            var totPts = 0.0M;

            for ( var w = Constants.K_WEEKS_IN_REGULAR_SEASON; w > 0; w-- )
            {
               string theWeek = string.Format( "{0:0#}", w );
               var ds = Utility.TflWs.GameForTeam( Season, theWeek, team.TeamCode );
               if ( ds.Tables[ 0 ].Rows.Count != 1 )
                  continue;

               var tePts = CalculateFpts( team, theWeek, ds );
               totPts += tePts;
               var fieldName = string.Format( FieldFormat, theWeek );

               teamRow[ fieldName ] = tePts;
            }
            teamRow[ "TOTAL" ] = totPts;
            Data.Rows.Add( teamRow );

#if DEBUG2
            tCount++;
            if (tCount > 1)
               break;
#endif
         }
      }

      private decimal CalculateFpts( NflTeam team, string theWeek, DataSet ds )
      {
         // Process Stats and Scores for the week
         // save the calculations
         var game = new NFLGame( ds.Tables[ 0 ].Rows[ 0 ] );

         List<NFLPlayer> playerList = new List<NFLPlayer>();
         if ( game.IsAway( team.TeamCode ) )
            playerList = game.LoadAllFantasyAwayPlayers( Constants.K_RECEIVER_CAT );
         else
            playerList = game.LoadAllFantasyHomePlayers( Constants.K_RECEIVER_CAT );

         var tePts = 0.0M;
         var week = new NFLWeek( Season, theWeek );

         tePts = TallyPts( playerList, week );

         return tePts;
      }

      private static decimal TallyPts( 
         List<NFLPlayer> playerList, 
         NFLWeek week )
      {
         var tePts = 0.0M;
         var scorer = new YahooScorer( week );
         foreach ( var p in playerList )
         {
            if ( p.IsTe() )
               tePts += scorer.RatePlayer( p, week );
         }
         return tePts;
      }

      private static string TotalBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 60 )
            sColour = "RED";
         else if ( theValue < 80 )
            sColour = "GREEN";
         else
            sColour = "YELLOW";
         return sColour;
      }

      private static string TeBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 5 )
            sColour = "RED";
         else if ( theValue < 10 )
            sColour = "GREEN";
         else
            sColour = "YELLOW";
         return sColour;
      }

      public override string OutputFilename()
      {
         return FileOut;
      }
   }
}
