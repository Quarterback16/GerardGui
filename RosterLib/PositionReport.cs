using RosterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace RosterLib
{
   public class PositionReport : TeamReport
   {
      public List<PositionReportOptions> Options { get; set; }
      public string PositionAbbr { get; set; }
      public string PositionCategory { get; set; }

      Func<NFLPlayer, bool> PositionDelegate;

      public PositionReport( 
         IKeepTheTime timekeeper, 
         PositionReportOptions config
          ) : base()
      {
         Options = new List<PositionReportOptions>();
         Options.Add( config );
      }

      public PositionReport(
         IKeepTheTime timekeeper
          ) : base()
      {
         Options = new List<PositionReportOptions>();
         var config = new PositionReportOptions();
         config.Topic = "Tight Ends";
         config.PositionCategory = Constants.K_RECEIVER_CAT;
         config.PosDelegate = IsTe;
         Options.Add( config );
      }

      public bool IsTe( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_RECEIVER_CAT ) && p.Contains( "TE", p.PlayerPos );
      }


      public override void RenderAsHtml()
      {
         foreach ( var item in Options )
         {
            Name = $"{item.Topic} Report";
            Heading = Name;
            PositionCategory = item.PositionCategory;
            PositionDelegate = item.PosDelegate;
            FileOut = string.Format( "{0}{1}//Scores//{2}-Scores.htm",
                        Utility.OutputDirectory(), Season, item.PositionAbbr );
            RenderSingle();
         }
      }

      private void RenderSingle()
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
         ReportColumn.ColourDelegate totalColourDelegateIn = PickTotalColourDelegate();
         var str = new SimpleTableReport( Heading );
         str.ColumnHeadings = true;
         str.DoRowNumbers = true;
         str.AddColumn( new ReportColumn( "Team", "TEAM", "{0,-20}" ) );
         str.AddColumn( new ReportColumn( "Total", "TOTAL", "{0:0.00}", typeof( decimal ), tally: true,
            colourDelegateIn: totalColourDelegateIn ) );

         var startAt =  Constants.K_WEEKS_IN_REGULAR_SEASON;

         for ( var w = startAt; w > 0; w-- )
         {
            var header = string.Format( "Week {0}", w );
            var fieldName = string.Format( FieldFormat, w );
            ReportColumn.ColourDelegate colourDelegateIn = PickColourDelegate();
            str.AddColumn( new ReportColumn( header, fieldName, "{0,5}", colourDelegateIn ) );
         }
         return str;
      }

      private ReportColumn.ColourDelegate PickTotalColourDelegate()
      {
         ReportColumn.ColourDelegate theDelegate;
         switch ( PositionAbbr )
         {
            case "RB":
               theDelegate = TotRbBgPicker;
               break;

            case "WR":
               theDelegate = TotWrBgPicker;
               break;

            case "QB":
               theDelegate = TotQbBgPicker;
               break;

            case "PK":
               theDelegate = TotPkBgPicker;
               break;

            default:
               theDelegate = TotTeBgPicker;
               break;
         }
         return theDelegate;

      }

      private ReportColumn.ColourDelegate PickColourDelegate()
      {
         ReportColumn.ColourDelegate theDelegate;
         switch ( PositionAbbr )
         {
            case "RB":
               theDelegate = RbBgPicker;
               break;

            case "WR":
               theDelegate = WrBgPicker;
               break;

            case "QB":
               theDelegate = QbBgPicker;
               break;

            case "PK":
               theDelegate = PkBgPicker;
               break;

            default:
               theDelegate = TeBgPicker;
               break;
         }
         return theDelegate;
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

      private decimal CalculateFpts( NflTeam team, string theWeek, DataSet gameDs )
      {
         // Process Stats and Scores for the week
         // save the calculations
         var game = new NFLGame( gameDs.Tables[ 0 ].Rows[ 0 ] );

         List<NFLPlayer> playerList = new List<NFLPlayer>();
         if ( game.IsAway( team.TeamCode ) )
            playerList = game.LoadAllFantasyAwayPlayers( PositionCategory );
         else
            playerList = game.LoadAllFantasyHomePlayers( PositionCategory );

         var pts = 0.0M;
         var week = new NFLWeek( Season, theWeek );

         pts = TallyPts( playerList, week );

         //TODO: AddLine to break down and Dump Breakdown

         return pts;
      }

      private decimal TallyPts( 
         List<NFLPlayer> playerList, 
         NFLWeek week )
      {
         var pts = 0.0M;
         var scorer = new YahooScorer( week );
         foreach ( var p in playerList )
         {
            if ( PositionDelegate( p ) )
            {
               pts += scorer.RatePlayer( p, week );
               //TODO: AddLine to break down
            }
         }
         return pts;
      }

      private static string TotQbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 200 )
            sColour = "RED";
         else if ( theValue < 300 )
            sColour = "GREEN";
         else
            sColour = "YELLOW";
         return sColour;
      }

      private static string TotPkBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 70 )
            sColour = "RED";
         else if ( theValue < 140 )
            sColour = "GREEN";
         else
            sColour = "YELLOW";
         return sColour;
      }

      private static string TotTeBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 50 )
            sColour = "RED";
         else if ( theValue < 100 )
            sColour = "GREEN";
         else
            sColour = "YELLOW";
         return sColour;
      }

      private static string TotWrBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 200 )
            sColour = "RED";
         else if ( theValue < 400 )
            sColour = "GREEN";
         else
            sColour = "YELLOW";
         return sColour;
      }

      private static string TotRbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 100 )
            sColour = "RED";
         else if ( theValue < 350 )
            sColour = "GREEN";
         else
            sColour = "YELLOW";
         return sColour;
      }

      private static string QbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 10 )
            sColour = "RED";
         else if ( theValue < 20 )
            sColour = "GREEN";
         else
            sColour = "YELLOW";
         return sColour;
      }

      private static string PkBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 5 )
            sColour = "RED";
         else if ( theValue < 7 )
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

      private static string WrBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 10 )
            sColour = "RED";
         else if ( theValue < 25 )
            sColour = "GREEN";
         else
            sColour = "YELLOW";
         return sColour;
      }

      private static string RbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 10 )
            sColour = "RED";
         else if ( theValue < 20 )
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

   public class PositionReportOptions
   {
      public string Topic { get; set; }
      public Func<NFLPlayer,bool> PosDelegate { get; set; }
      public string PositionAbbr { get; set; }
      public string PositionCategory { get; set; }
   }
}
