using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NLog;
using RosterLib.Models;
using RosterLib.Repositories;
using RosterLib.Interfaces;

namespace RosterLib
{
   public class RoleAssignmentReport : RosterGridReport
   {
      public string SingleTeam { get; set; }

      public List<String> Lines { get; set; }

      public List<String> Aces { get; set; }

      public RoleAssignmentReport()
      {
         Initialise();
      }

      public RoleAssignmentReport(IKeepTheTime timekeeper)
      {
         Initialise(timekeeper);
      }

      private void Initialise(IKeepTheTime timekeeper)
      {
         Name = "Role Assignment Report";
         Lines = new List<String> { Name };
         Season = timekeeper.CurrentSeason( DateTime.Now );
         Aces = new List<string>();
         if ( Logger == null ) Logger = LogManager.GetCurrentClassLogger();
      }

      private void Initialise()
      {
         Name = "Role Assignment Report";
         Lines = new List<String> {Name};
         Season = TimeKeeper.CurrentSeason( DateTime.Now );
         Aces = new List<string>();
         if (Logger == null) Logger = LogManager.GetCurrentClassLogger();
      }

      public RoleAssignmentReport( string singleTeamCode )
      {
         SingleTeam = singleTeamCode;
         Initialise();
      }

      public override string OutputFilename()
      {
         return string.Format( "{0}{1}/{2}.htm", Utility.OutputDirectory(), Season, Name );
      }

      public override void RenderAsHtml()
      {
         var aceRepository = new AceRepository();
         var teamLines = new List<String>();
         var week = Utility.PreviousWeekAsString();
         Logger.Trace( string.Format( "Analysing roles from week {0}", week  ) );

         List<NflTeam> teamList;
         if (string.IsNullOrEmpty( SingleTeam ))
         {
            var s = new NflSeason( Season, true );
            teamList = s.TeamList;
         }
         else
            teamList = new List<NflTeam> {new NflTeam( SingleTeam )};

         foreach ( var t in teamList )
         {
            teamLines.Clear();
            teamLines.Add( "------------------------------------------------------------"  );
            teamLines.Add( t.NameOut() );
            teamLines.Add( "------------------------------------------------------------" );
            t.LoadRushUnit();
            teamLines.Add( "Runningbacks" + Environment.NewLine );
            teamLines.AddRange( t.RushUnit.LoadCarries( Season, week ) );
            teamLines.Add( Environment.NewLine );

            if (t.RushUnit.IsAceBack && t.RushUnit.AceBack.TotStats.Touches > 10)
               AddAceLine( t.RushUnit.AceBack, aceRepository );

            t.LoadPassUnit();
            teamLines.Add( "Quarterbacks" + Environment.NewLine );
            teamLines.AddRange( t.PassUnit.AnalyseQuarterbacks( Season, week ) );
            teamLines.Add( Environment.NewLine );

            teamLines.Add( "Wideouts" + Environment.NewLine );
            teamLines.AddRange( t.PassUnit.AnalyseWideouts( Season, week ) );
            teamLines.Add( Environment.NewLine );

            if ( t.PassUnit.IsAceReceiver && t.PassUnit.AceReceiver.TotStats.Touches > 5 )
               AddAceLine(t.PassUnit.AceReceiver, aceRepository);

            teamLines.Add( "Tight Ends" + Environment.NewLine );
            teamLines.AddRange( t.PassUnit.AnalyseTightends( Season, week ) );
            teamLines.Add( Environment.NewLine );

            if ( t.PassUnit.IsAceTightEnd && t.PassUnit.AceTightEnd.TotStats.Touches > 5 )
               AddAceLine(t.PassUnit.AceTightEnd, aceRepository);

            Lines.AddRange( teamLines );
            DumpTeam( teamLines, week, t );
#if DEBUG2
            break;
#endif
         }
         DumpLines( week );
         Finish();
         DumpAces( week );
      }

      private void AddAceLine( NFLPlayer p, IAceRepository ar )
      {
         var dline = p.DetailLine();
         if (!string.IsNullOrEmpty( dline )) Aces.Add( dline );
         var ace = new Ace
         {
            PlayerId = p.PlayerCode,
            TeamCode = p.TeamCode,
            Season = Utility.CurrentSeason(),
            Week = Utility.PreviousWeek().ToString( CultureInfo.InvariantCulture ),
            PlayerCat = p.PlayerCat,
            Touches = p.TotStats.Touches,
            Load = p.TotStats.TouchLoad,
         };
         ar.Update(ace);
      }

      private void DumpTeam( IEnumerable<string> teamLines, string week, NflTeam t )
      {
         var PreReport = new SimplePreReport
            {
               ReportType = "Team Role Assignment",
               Folder = "Roles",
               Season = Season,
               InstanceName = string.Format( "{1}-Roles-{0:0#}", Int32.Parse( week ), t.TeamCode ),
               Body = GenerateErrorBody( teamLines )
            };
         PreReport.RenderHtml();
         FileOut = PreReport.FileOut;
      }

      public void DumpLines( string week )
      {
         var PreReport = new SimplePreReport
            {
               ReportType = "Role Assignment",
               Folder = "Roles",
               Season = Season,
               InstanceName = string.Format( "RoleAssignments-{0:0#}", Int32.Parse( week ) ),
               Body = GenerateErrorBody( Lines )
            };
         PreReport.RenderHtml();
         FileOut = PreReport.FileOut;
      }

      private static string GenerateErrorBody( IEnumerable<string> lines )
      {
         var line = 0;
         var bodyOut = new StringBuilder();
         foreach ( var err in lines )
         {
            line++;
            Utility.Announce( err );
            bodyOut.AppendLine( string .Format( "{0,3} {1}", line, err) );
         }

         bodyOut.AppendLine();

         return bodyOut.ToString();
      }

      public void DumpAces( string week )
      {
         var PreReport = new SimplePreReport
         {
            ReportType = "Aces",
            Folder = "Roles",
            Season = Season,
            InstanceName = string.Format( "Aces-{0:0#}", Int32.Parse( week ) ),
            Body = GenerateErrorBody( Aces )
         };
         PreReport.RenderHtml();
         FileOut = PreReport.FileOut;
      }
   }
}