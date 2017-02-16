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

      public string RootFolder { get; set; }

      Func<NFLPlayer, bool> PositionDelegate;

      public IBreakdown PlayerBreakdowns { get; set; }

      public PositionReport( 
         IKeepTheTime timekeeper, 
         PositionReportOptions config
          ) : base()
      {
         Options = new List<PositionReportOptions>();
         Options.Add( config );
         PlayerBreakdowns = new PreStyleBreakdown();
      }

      public PositionReport(
         IKeepTheTime timekeeper
          ) : base()
      {
         Options = new List<PositionReportOptions>();
         LoadAllTheOptions();
         PlayerBreakdowns = new PreStyleBreakdown();
      }

      public void LoadAllTheOptions()
      {
         AddQuarterbackReport();
         AddRunningBackReport();
         AddWideReceiverReport();
         AddTightEndReport();
         AddKickerReport();
      }

      private void AddTightEndReport()
      {
         var config = new PositionReportOptions();
         config.Topic = "Tight Ends";
         config.PositionCategory = Constants.K_RECEIVER_CAT;
         config.PosDelegate = IsTe;
         Options.Add( config );
      }

      private void AddWideReceiverReport()
      {
         var config = new PositionReportOptions();
         config.Topic = "Wide Receivers";
         config.PositionCategory = Constants.K_RECEIVER_CAT;
         config.PosDelegate = IsWr;
         Options.Add( config );
      }

      private void AddRunningBackReport()
      {
         var config = new PositionReportOptions();
         config.Topic = "Running Backs";
         config.PositionCategory = Constants.K_RUNNINGBACK_CAT;
         config.PosDelegate = IsRb;
         Options.Add( config );
      }

      private void AddQuarterbackReport()
      {
         var config = new PositionReportOptions();
         config.Topic = "Quarterbacks";
         config.PositionCategory = Constants.K_QUARTERBACK_CAT;
         config.PosDelegate = IsQb;
         Options.Add( config );
      }

      private void AddKickerReport()
      {
         var config = new PositionReportOptions();
         config.Topic = "Kickers";
         config.PositionCategory = Constants.K_KICKER_CAT;
         config.PosDelegate = IsPk;
         Options.Add( config );
      }

      public bool IsTe( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_RECEIVER_CAT ) && p.Contains( "TE", p.PlayerPos );
      }

      public bool IsWr( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_RECEIVER_CAT ) && p.Contains( "WR", p.PlayerPos );
      }

      public bool IsRb( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_RUNNINGBACK_CAT ) && p.Contains( "RB", p.PlayerPos );
      }

      public bool IsQb( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_QUARTERBACK_CAT ) && p.Contains( "QB", p.PlayerPos );
      }

      public bool IsPk( NFLPlayer p )
      {
         return ( p.PlayerCat == Constants.K_KICKER_CAT );
      }

      public override void RenderAsHtml()
      {
         foreach ( var item in Options )
         {
            Name = $"{item.Topic} Report";
            Heading = Name;
            PositionCategory = item.PositionCategory;
            PositionDelegate = item.PosDelegate;
            PositionAbbr = item.PositionAbbr;
            RootFolder = string.Format( "{0}{1}//Scores//",
                        Utility.OutputDirectory(), Season, item.PositionAbbr );
            FileOut = string.Format( "{0}{1}-Scores.htm", RootFolder, item.PositionAbbr );
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

               var pts = CalculateFpts( team, theWeek, ds );
               totPts += pts;
               var fieldName = string.Format( FieldFormat, theWeek );

               teamRow[ fieldName ] = LinkFor( team.TeamCode, theWeek, pts);
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

      private string LinkFor( string teamCode, string theWeek, decimal pts )
      {
         var link = $"<a href='.//breakdowns//{teamCode}-{PositionAbbr}-{theWeek}.htm'>{pts}";
         return link;
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

         pts = TallyPts( playerList, week, team.TeamCode );

         //TODO: AddLine to break down and Dump Breakdown
         return pts;
      }

      private decimal TallyPts( 
         List<NFLPlayer> playerList, 
         NFLWeek week,
         string teamCode )
      {
         var pts = 0.0M;
         var scorer = new YahooScorer( week );
         var breakDownKey = $"{teamCode}-{PositionAbbr}-{week.Week}";
         foreach ( var p in playerList )
         {
            if ( PositionDelegate( p ) )
            {
               var plyrPts = scorer.RatePlayer( p, week );
               if ( plyrPts != 0 )
               {
                  PlayerBreakdowns.AddLine( breakDownKey,
                     line: $"{p.PlayerName} {plyrPts.ToString()}" );
               }
               pts += plyrPts;
            }
         }
         PlayerBreakdowns.Dump( breakDownKey, $"{RootFolder}\\breakdowns\\{breakDownKey}.htm" );
         return pts;
      }

      private static string TotQbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 250 )
            sColour = Constants.Colour.Bad;
         else if ( theValue < 290 )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Good;
         return sColour;
      }

      private static string TotPkBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 100 )
            sColour = Constants.Colour.Bad; 
         else if ( theValue < 145 )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Good;
         return sColour;
      }

      private static string TotTeBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 50 )
            sColour = Constants.Colour.Bad;
         else if ( theValue < 100 )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Good;
         return sColour;
      }

      private static string TotWrBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 325 )
            sColour = Constants.Colour.Bad;
         else if ( theValue < 390 )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Good;
         return sColour;
      }

      private static string TotRbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 220 )
            sColour = Constants.Colour.Bad;
         else if ( theValue < 325 )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Good;
         return sColour;
      }

      private static string QbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 10 )
            sColour = Constants.Colour.Bad;
         else if ( theValue < 20 )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Good;
         return sColour;
      }

      private static string PkBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 5 )
            sColour = Constants.Colour.Bad;
         else if ( theValue < 7 )
            sColour = Constants.Colour.Average;
         else if ( theValue < 20 )
            sColour = Constants.Colour.Good;
         else
            sColour = Constants.Colour.Excellent;  // Extremely good
         return sColour;
      }

      private static string TeBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 5 )
            sColour = Constants.Colour.Bad;
         else if ( theValue < 10 )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Good;
         return sColour;
      }

      private static string WrBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 10 )
            sColour = Constants.Colour.Bad;
         else if ( theValue < 25 )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Good;
         return sColour;
      }

      private static string RbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < 10 )
            sColour = Constants.Colour.Bad;
         else if ( theValue < 20 )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Good;
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
