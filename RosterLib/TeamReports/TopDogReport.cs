using RosterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace RosterLib.TeamReports
{
	public class TopDogReport : TeamReport
	{
		public string Week { get; private set; }

		public List<TopDogReportOptions> Options { get; set; }

		public string PositionAbbr { get; set; }
		public string PositionCategory { get; set; }
		public string RootFolder { get; set; }

		public TopDogReport( IKeepTheTime timekeeper ) : base()
		{
			Options = new List<TopDogReportOptions>();
			LoadAllTheOptions();
			SetTime( timekeeper );
		}

		public void LoadAllTheOptions()
		{
			//AddQuarterbackReport();
			AddRunningBackReport();
#if DONE
			AddWideReceiverReport();
			AddTightEndReport();
			AddKickerReport();
#endif
		}

#if DONE
		private void AddTightEndReport()
		{
			var config = new TopDogReportOptions()
			{
				Topic = "Tight Ends",
				PositionCategory = Constants.K_RECEIVER_CAT
			};
			Options.Add( config );
		}

		private void AddWideReceiverReport()
		{
			var config = new TopDogReportOptions
			{
				Topic = "Wide Receivers",
				PositionCategory = Constants.K_RECEIVER_CAT
			};
			Options.Add( config );
		}

		private void AddRunningBackReport()
		{
			var config = new TopDogReportOptions
			{
				Topic = "Running Backs",
				PositionCategory = Constants.K_RUNNINGBACK_CAT
			};
			Options.Add( config );
		}

		private void AddKickerReport()
		{
			var config = new TopDogReportOptions
			{
				Topic = "Kickers",
				PositionCategory = Constants.K_KICKER_CAT
			};
			Options.Add( config );
		}

#endif

		private void AddRunningBackReport()
		{
			var config = new TopDogReportOptions
			{
				Topic = "Runningbacks",
				PositionAbbr = "RB",
				PositionCategory = Constants.K_RUNNINGBACK_CAT
			};
			Options.Add( config );
		}

		private void AddQuarterbackReport()
		{
			var config = new TopDogReportOptions
			{
				Topic = "Quarterbacks",
				PositionAbbr = "QB",
				PositionCategory = Constants.K_QUARTERBACK_CAT
			};
			Options.Add( config );
		}

		private void SetTime( IKeepTheTime timekeeper )
		{
			Season = timekeeper.Season;
			Week = timekeeper.Week;
		}

		public override void RenderAsHtml()
		{
			foreach ( var item in Options )
			{
				Name = $"{item.Topic} Top Dog Report";
				Heading = Name;
				PositionCategory = item.PositionCategory;
				PositionAbbr = item.PositionAbbr;
				RootFolder = $"{Utility.OutputDirectory()}{Season}//Starters//";
				FileOut = string.Format( "{0}{1}-topdogs-{2}.htm",
				   RootFolder, item.PositionAbbr, Week );
				RenderSingle();
			}
		}

		private void RenderSingle()
		{
			// Could possibly use the Template Pattern here

			Ste = DefineSte();

			Data = BuildDataTable();

			LoadDataTable();

			Render();

			Finish();
		}

		private const string FieldFormat = "Wk{0:0#}";

		private SimpleTableReport DefineSte()
		{
			var str = new SimpleTableReport( Heading )
			{
				ColumnHeadings = true,
				DoRowNumbers = true
			};
			str.AddColumn( new ReportColumn( "Team", "TEAM", "{0,-20}" ) );
			str.AddColumn( new ReportColumn( "Total", "TOTAL", "{0:0.00}", typeof( decimal ), tally: true ) );

			for ( var w = 1; w <= Constants.K_WEEKS_IN_REGULAR_SEASON; w++ )
			{
				var header = $"Week {w}";
				var fieldName = string.Format( FieldFormat, w );
				str.AddColumn( new ReportColumn( header, fieldName, "{0,5}", PickColourDelegate() ) );
			}
			return str;
		}

		private ReportColumn.ColourDelegate PickColourDelegate()
		{
			ReportColumn.ColourDelegate theDelegate;
			switch ( PositionAbbr )
			{
#if DONE

				case "WR":
					theDelegate = WrBgPicker;
					break;
#endif
				case "QB":
					theDelegate = QbBgPicker;
					break;

				case "RB":
					theDelegate = RbBgPicker;
					break;

#if DONE
				case "PK":
					theDelegate = PkBgPicker;
					break;
#endif
				default:
					theDelegate = QbBgPicker;
					break;
			}
			return theDelegate;

		}

		private string RbBgPicker( int theValue )
		{
			var colourTable = new Dictionary<string, decimal>()
			{
				[ Constants.Colour.Bad ] = 5,
				[ Constants.Colour.Average ] = 10,
				[ Constants.Colour.Good ] = Decimal.MaxValue,
			};
			return GetColourFor( theValue, colourTable );
		}

		private string QbBgPicker( int theValue )
		{
			var colourTable = new Dictionary<string, decimal>()
			{
				[ Constants.Colour.Bad ] = 10,
				[ Constants.Colour.Average ] = 20,
            [ Constants.Colour.Good ] = Decimal.MaxValue,
         };
			return GetColourFor(theValue, colourTable);
		}

		private string GetColourFor( int theValue, Dictionary<string, decimal> colourTable )
		{
			var theColour = Constants.Colour.Default;
         if ( theValue > 0 )
         {
            foreach ( KeyValuePair<string, decimal> pair in colourTable )
            {
               if ( theValue <= pair.Value )
               {
                  theColour = pair.Key;
                  break;
               }
            }
         }
			return theColour;
		}

		private static DataTable BuildDataTable()
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
				var teamRow = Data.NewRow();
				teamRow[ "TEAM" ] = team.DepthChartLink();
				teamRow[ "TOTAL" ] = 0;
				var totPts = 0.0M;

				for ( var w = Constants.K_WEEKS_IN_REGULAR_SEASON; w > 0; w-- )
				{
					var theWeek = $"{w:0#}";
					var fieldName = string.Format( FieldFormat, theWeek );
					var ds = Utility.TflWs.GameForTeam( Season, theWeek, team.TeamCode );
					if ( ds.Tables[ 0 ].Rows.Count != 1 )
					{
						teamRow[ fieldName ] = "BYE";
					}
					else
					{
						var topDog = TopDog( team, theWeek, ds );
						var pts = topDog.Points;
						totPts += pts;
						teamRow[ fieldName ] = LinkFor( topDog, Convert.ToInt32(pts) );
					}
				}
				teamRow[ "TOTAL" ] = totPts / Constants.K_WEEKS_IN_REGULAR_SEASON;
				Data.Rows.Add( teamRow );
			}
		}

		private static string LinkFor( NFLPlayer topDog, int pts )
		{
			var link = $"{pts} : <a href='..//..//Players//{topDog.PlayerCode}.htm'>{topDog.PlayerNameShort}";
			return link;
		}

		private NFLPlayer TopDog( NflTeam team, string theWeek, DataSet gameDs )
		{
			var game = new NFLGame( gameDs.Tables[ 0 ].Rows[ 0 ] );
			var week = new NFLWeek( Season, theWeek );
			var scorer = new YahooXmlScorer( week );
			List<NFLPlayer> playerList = new List<NFLPlayer>();
			if ( game.IsAway( team.TeamCode ) )
				playerList = game.LoadAllFantasyAwayPlayers( PositionCategory );
			else
				playerList = game.LoadAllFantasyHomePlayers( PositionCategory );
			NFLPlayer topDog = new NFLPlayer( "MONTJO01" )
			{
				Points = 0
			};
			foreach ( var p in playerList )
			{
				p.Points = scorer.RatePlayer( p, week );
				if ( p.Points > topDog.Points )
					topDog = p;
			}
			return topDog;
		}
	}

	public class TopDogReportOptions
	{
		public string Topic { get; set; }
		public string PositionAbbr { get; set; }
		public string PositionCategory { get; set; }
	}
}
