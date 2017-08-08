﻿using RosterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace RosterLib
{
   public class PointsAllowedReport : TeamReport
   {
      public string Week { get; set; }
      public string RootFolder { get; set; }
      public IBreakdown TeamBreakdowns { get; set; }

      public PointsAllowedReport( IKeepTheTime timekeeper ) : base( timekeeper )
		{
         Week = timekeeper.Week;
         if ( timekeeper.IsItPostSeason() )
         {
            Week = Constants.K_GAMES_IN_REGULAR_SEASON.ToString();
         }
         TeamBreakdowns = new PreStyleBreakdown();
      }

      public override void RenderAsHtml()
      {
         Name = "Points Allowed Report";
         Heading = $"{Name} Week {Season}:{Week}";
         RootFolder = $"{Utility.OutputDirectory()}{Season}//Scores//";
         FileOut = string.Format( "{0}Points-Allowed-{1}.htm", RootFolder, Week );
         RenderSingle();
      }

      private void RenderSingle()
      {
         Ste = DefineSte();
         Data = BuildDataTable();

         LoadDataTable();

         Render();

         Finish();
      }

      private ReportColumn.ColourDelegate PickTotalColourDelegate( string positionAbbr )
      {
         ReportColumn.ColourDelegate theDelegate;
         switch ( positionAbbr )
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

            case "TE":
               theDelegate = TotTeBgPicker;
               break;

            default:
               theDelegate = TotBgPicker;
               break;
         }
         return theDelegate;

      }

      private SimpleTableReport DefineSte()
      {
         var str = new SimpleTableReport( Heading );
         str.ColumnHeadings = true;
         str.DoRowNumbers = true;
         str.AddColumn( new ReportColumn( "Team", "TEAM", "{0,-20}" ) );
         str.AddColumn( 
            new ReportColumn( "Total", "TOTAL", "{0:0.00}", typeof( decimal ), tally: true,
            colourDelegateIn: PickTotalColourDelegate("TOT") ) );
         str.AddColumn( 
            new ReportColumn( "QB", "QB", "{0:0.00}", typeof( decimal ), tally: true,
            colourDelegateIn: PickTotalColourDelegate( "QB" ) ) );
         str.AddColumn(
            new ReportColumn( "RB", "RB", "{0:0.00}", typeof( decimal ), tally: true,
            colourDelegateIn: PickTotalColourDelegate( "RB" ) ) );
         str.AddColumn(
            new ReportColumn( "WR", "WR", "{0:0.00}", typeof( decimal ), tally: true,
            colourDelegateIn: PickTotalColourDelegate( "WR" ) ) );
         str.AddColumn(
            new ReportColumn( "TE", "TE", "{0:0.00}", typeof( decimal ), tally: true,
            colourDelegateIn: PickTotalColourDelegate( "TE" ) ) );
         str.AddColumn(
            new ReportColumn( "PK", "PK", "{0:0.00}", typeof( decimal ), tally: true,
            colourDelegateIn: PickTotalColourDelegate( "PK" ) ) );

         return str;
      }

      private DataTable BuildDataTable()
      {
         var dt = new DataTable();
         var cols = dt.Columns;
         cols.Add( "TEAM", typeof( String ) );
         cols.Add( "TOTAL", typeof( decimal ) );
         cols.Add( "QB", typeof( string ) );
         cols.Add( "RB", typeof( string ) );
         cols.Add( "WR", typeof( string ) );
         cols.Add( "TE", typeof( string ) );
         cols.Add( "PK", typeof( string ) );

         dt.DefaultView.Sort = "TOTAL DESC";
         return dt;
      }

      private void LoadDataTable()
      {
#if DEBUG2
         var tCount = 0;
#endif
         var asOfWeek = int.Parse( Week );
         foreach ( KeyValuePair<string, NflTeam> teamPair in TeamList )
         {
            var team = teamPair.Value;
            DataRow teamRow = Data.NewRow();
            teamRow[ "TEAM" ] = team.NameOut();
            teamRow[ "TOTAL" ] = 0;
            FptsAllowed fptsAllowed = new FptsAllowed();
            FptsAllowed totalFptsAllowed = new FptsAllowed();

            for ( var w = Constants.K_WEEKS_IN_REGULAR_SEASON; w > 0; w-- )
            {
               if ( w > asOfWeek ) continue;
               string theWeek = string.Format( "{0:0#}", w );
               var ds = Utility.TflWs.GameForTeam( Season, theWeek, team.TeamCode );
               if ( ds.Tables[ 0 ].Rows.Count != 1 )
                  continue;

               fptsAllowed = CalculateFptsAllowed( team, theWeek, ds );
               totalFptsAllowed.Add( fptsAllowed );

            }

            teamRow[ "QB" ] = LinkFor( team.TeamCode, "QB", totalFptsAllowed.ToQbs);
            teamRow[ "RB" ] = LinkFor( team.TeamCode, "RB", totalFptsAllowed.ToRbs);
            teamRow[ "WR" ] = LinkFor( team.TeamCode, "WR", totalFptsAllowed.ToWrs);
            teamRow[ "TE" ] = LinkFor( team.TeamCode, "TE", totalFptsAllowed.ToTes);
            teamRow[ "PK" ] = LinkFor( team.TeamCode, "PK", totalFptsAllowed.ToPks);

            teamRow[ "TOTAL" ] = totalFptsAllowed.TotPtsAllowed();
            Data.Rows.Add( teamRow );
            DumpBreakdowns( team.TeamCode );

#if DEBUG2
            tCount++;
            if ( tCount > 0 )
               break;
#endif
         }
      }

      private void DumpBreakdowns( string teamCode )
      {
         DumpBreakdown( teamCode, "QB" );
         DumpBreakdown( teamCode, "RB" );
         DumpBreakdown( teamCode, "WR" );
         DumpBreakdown( teamCode, "TE" );
         DumpBreakdown( teamCode, "PK" );
      }

      private void DumpBreakdown( string teamCode, string positionAbbr )
      {
         var breakdownKey = $"{teamCode}-{positionAbbr}-{Week}";
         TeamBreakdowns.Dump( breakdownKey,
            $"{RootFolder}\\pts-allowed\\{breakdownKey}.htm" );
      }

      private string LinkFor( string teamCode, string positionAbbr, decimal pts )
      {
         var link = $"<a href='.//pts-allowed//{teamCode}-{positionAbbr}-{Week}.htm'>{pts}";
         return link;
      }

      private FptsAllowed CalculateFptsAllowed( 
         NflTeam team, string theWeek, DataSet gameDs )
      {
         // Process Stats and Scores for the week
         // save the calculations
         var ftpsAllowed = new FptsAllowed();
         var game = new NFLGame( gameDs.Tables[ 0 ].Rows[ 0 ] );

         List<NFLPlayer> playerList = new List<NFLPlayer>();
         if ( game.IsAway( team.TeamCode ) )
            playerList = game.LoadAllFantasyHomePlayers();
         else
            playerList = game.LoadAllFantasyAwayPlayers();

         var week = new NFLWeek( Season, theWeek );

         var scorer = new YahooXmlScorer( week );
         foreach ( var p in playerList )
         {
            var plyrPts = scorer.RatePlayer( p, week );

            if ( p.IsQuarterback() )
            {
               ftpsAllowed.ToQbs += plyrPts;
               AddBreakdownLine( team, theWeek, p, plyrPts, "QB" );
            }
            else if ( p.IsRb() )
            {
               ftpsAllowed.ToRbs += plyrPts;
               AddBreakdownLine( team, theWeek, p, plyrPts, "RB" );
            }
            else if ( p.IsWideout() )
            {
               ftpsAllowed.ToWrs += plyrPts;
               AddBreakdownLine( team, theWeek, p, plyrPts, "WR" );
            }
            else if ( p.IsTe() )
            {
               ftpsAllowed.ToTes += plyrPts;
               AddBreakdownLine( team, theWeek, p, plyrPts, "TE" );
            }
            else if ( p.IsKicker() )
            {
               ftpsAllowed.ToPks += plyrPts;
               AddBreakdownLine( team, theWeek, p, plyrPts, "PK" );
            }
         }

         return ftpsAllowed;
      }

      private void AddBreakdownLine( 
         NflTeam team, string theWeek, NFLPlayer p, decimal plyrPts, string abbr )
      {
         if ( plyrPts == 0 ) return;

         var strPts = string.Format( "{0:0.0}", plyrPts );
         strPts = strPts.PadLeft( 5 );
         strPts = strPts.Substring(strPts.Length - 5);
         TeamBreakdowns.AddLine(
            breakdownKey: $"{ team.TeamCode}-{abbr}-{Week}",
            line: $@"Wk:{
               theWeek
               } {p.PlayerName,-25} Pts : {strPts}"
             );
      }

      private decimal FractionOfTheSeason()
      {
         var multiplier = decimal.Parse( Week ) / Constants.K_WEEKS_IN_REGULAR_SEASON;
         return multiplier;
      }

      private string TotQbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < (250 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Good;
         else if ( theValue < (290 * FractionOfTheSeason()) )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Bad;
         return sColour;
      }

      private string TotBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < (1035 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Good;
         else if ( theValue < (1165 * FractionOfTheSeason()) )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Bad;
         return sColour;
      }

      private string TotPkBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < ( 100 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Good;
         else if ( theValue < ( 145 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Bad;
         return sColour;
      }

      private string TotTeBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < ( 100 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Good;
         else if ( theValue < ( 130 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Bad;
         return sColour;
      }

      private string TotWrBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < ( 320 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Good;
         else if ( theValue < ( 380 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Bad;
         return sColour;
      }

      private string TotRbBgPicker( int theValue )
      {
         string sColour;

         if ( theValue < ( 225 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Good;
         else if ( theValue < ( 300 * FractionOfTheSeason() ) )
            sColour = Constants.Colour.Average;
         else
            sColour = Constants.Colour.Bad;
         return sColour;
      }

      public override string OutputFilename()
      {
         return FileOut;
      }

   }

   public class FptsAllowed
   {
      public decimal ToQbs { get; set; }
      public decimal ToRbs { get; set; }
      public decimal ToWrs { get; set; }
      public decimal ToTes { get; set; }
      public decimal ToPks { get; set; }

      internal decimal TotPtsAllowed()
      {
         return ToQbs + ToRbs + ToWrs + ToTes + ToPks;
      }

      internal void Add( FptsAllowed fptsAllowed )
      {
         ToQbs += fptsAllowed.ToQbs;
         ToRbs += fptsAllowed.ToRbs;
         ToWrs += fptsAllowed.ToWrs;
         ToTes += fptsAllowed.ToTes;
         ToPks += fptsAllowed.ToPks;
      }

   }
}

