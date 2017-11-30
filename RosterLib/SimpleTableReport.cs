﻿///////////////////////////////////////////////////////////
//  SimpleTableReport.cs
//  Implementation of the Class SimpleTableReport
//  Generated by Enterprise Architect
//  Created on:      25-Jul-2005 17:19:14
//  Original author: Steve Colonna
///////////////////////////////////////////////////////////

using NLog;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

//using Excel = Microsoft.Office.Interop.Excel;

namespace RosterLib
{
	public class SimpleTableReport
	{
		private readonly string _header;

		/// <summary>
		/// DataRows to be reported on.
		/// </summary>
		private DataTable _body;

		private readonly DataColumnCollection _cols;
		private readonly string _footer;
		private readonly ArrayList _columns;

		public string CustomHeader { get; set; }

		public string FootNote { get; set; }

		private bool _rowNumbers;
		private bool _showElapTime = true;
		private bool _carryRow = true;
		private bool _isfooter = true;

		private string _timeTaken = "";
		private string _subHeader = "";

		private readonly ElapsedTimer _et;

		public bool AnnounceIt { get; set; }

		#region CONSTRUCTORS

		public SimpleTableReport()
		{
			_columns = new ArrayList();
			_et = new ElapsedTimer();
			_et.Start( DateTime.Now );
			StyleList = new ArrayList();
			_body = new DataTable();
			_cols = Body.Columns;
			AnnounceIt = true;
		}

		/// <summary>
		///   Report with a header and a footer
		/// </summary>
		/// <param name="header"></param>
		/// <param name="footer"></param>
		public SimpleTableReport( string header, string footer )
		{
			ReportHeader = header;
			ReportFooter = footer;
			_columns = new ArrayList();
			_et = new ElapsedTimer();
			_et.Start( DateTime.Now );
			StyleList = new ArrayList();
			_body = new DataTable();
			_cols = Body.Columns;
			SubHeader = string.Empty;
		}

		/// <summary>
		///   No footer
		/// </summary>
		/// <param name="header"></param>
		public SimpleTableReport( string header )
		{
			_header = header;
			_footer = String.Empty;
			_columns = new ArrayList();
			_et = new ElapsedTimer();
			_et.Start( DateTime.Now );
			StyleList = new ArrayList();
			_body = new DataTable();
			_cols = _body.Columns;
			SubHeader = string.Empty;

			//RosterLib.Utility.Announce(string.Format("SimpleTableReport {0} created", header ));
		}

		#endregion CONSTRUCTORS

		#region Accessors

		public string TimeTaken
		{
			get { return _timeTaken; }
			set { _timeTaken = value; }
		}

		public bool ShowElapsedTime
		{
			get { return _showElapTime; }
			set { _showElapTime = value; }
		}

		public bool CarryRow
		{
			get { return _carryRow; }
			set { _carryRow = value; }
		}

		public bool DoRowNumbers
		{
			get { return _rowNumbers; }
			set { _rowNumbers = value; }
		}

		public bool ColumnHeadings { get; set; }

		public string ReportHeader { get; set; }

		public string ReportFooter { get; set; }

		public bool Totals { get; set; }

		public int LastTotal { get; set; }

		public string SubHeader
		{
			get { return _subHeader; }
			set { _subHeader = value; }
		}

		public ArrayList StyleList { get; set; }

		public bool IsFooter
		{
			get { return _isfooter; }
			set { _isfooter = value; }
		}

		/// <summary>
		/// DataRows to be reported on.
		/// </summary>
		public DataTable Body
		{
			get { return _body; }
			set { _body = value; }
		}

		#endregion Accessors

		#region Output

		#region HTML Rendition

		/// <summary>
		///   Express the report in HTML.
		/// </summary>
		/// <param name="fileName">The output DOS file fame.  Include a directory.</param>
		/// <param name="persist">Whether to delete the file or not, sometimes we just want the string.</param>
		public string RenderAsHtml( string fileName, bool persist )
		{
			if ( ReportHeader == null ) ReportHeader = _header;
			ReportHeader = string.Format( "{0} as of {1}",
			   ReportHeader, DateTime.Now.ToString( "ddd dd MMM yy HH:MM tt" ) );
			var h = new HtmlFile( fileName, ReportHeader ) { AnnounceIt = AnnounceIt };
			AddStyles( h );
			var html = string.Format( "<h3>{0}</h3>", ReportHeader ) + Header( _header );
			if ( SubHeader.Length > 0 ) html += SubHeader;
			html += BodyOut();
			h.AddToBody( html );
			_et.Stop( DateTime.Now );
			TimeTaken = _et.TimeOut();
			if ( !string.IsNullOrEmpty( FootNote ) )
				h.AddToBody( FootNote );
			h.AddToBody( IsFooter ? ReportFooter : Footer() );
			if ( persist ) h.Render();
			return html;
		}

		private void AddStyles( HtmlFile h )
		{
			var styleEnumerator = StyleList.GetEnumerator();
			while ( styleEnumerator.MoveNext() )
				h.AddStyle( styleEnumerator.Current.ToString() );
		}

		public void LoadBody( DataTable data )
		{
			Body = data;
		}

		public void SetFilter( string filt )
		{
			Body.DefaultView.RowFilter = filt;
		}

		public void SetSortOrder( string order )
		{
			Body.DefaultView.Sort = order;
		}

		public void AddColumn( ReportColumn c )
		{
			_columns.Add( c );

			if ( c.Type != null )
				_cols.Add( c.Source, c.Type );
		}

		public void AddStyle( string style )
		{
			StyleList.Add( style );
		}

		public string BodyOut()
		{
			var rowCount = 0;
			var bBlank = false;
			var tot = new int[ _columns.Count ];
			for ( var i = 0; i < _columns.Count; i++ )
				tot[ i ] = 0;

			var sLastData = "";
			var s = "";

			if ( Body != null )
			{
				s += HtmlLib.TableOpen( "border=1 cellpadding='3' cellspacing='3'" );
				s += ColHeaders();
				//  now just add a series of rows for each record
				for ( var j = 0; j < Body.DefaultView.Count; j++ )
				{
					rowCount++;
					if ( IsEven( rowCount ) )
						s += HtmlLib.TableRowOpen( "BGCOLOR='MOCCASIN'" );
					else
						s += HtmlLib.TableRowOpen();

					if ( DoRowNumbers )
						s += HtmlLib.TableDataAttr( 
                            rowCount.ToString( 
                                CultureInfo.InvariantCulture ), 
                            "ALIGN='RIGHT' VALIGN='TOP'" );

					//  plug in the data for each column defined
					for ( var i = 0; i < _columns.Count; i++ )
					{
						var col = ( ReportColumn ) _columns[ i ];
						var dc = Body.Columns[ col.Source ];

						var sVal = Body.DefaultView[ j ][ col.Source ].ToString();
						var sData = FormatData( dc, col.Format, sVal );

						if ( col.CanAccumulate )
						{
							Totals = true;
							if ( sVal.Length > 0 ) tot[ i ] += QuantityOf( sVal );
						}

						if ( i == 0 )
						{
							if ( sData == sLastData )
								bBlank = true;
							else
							{
								sLastData = sData;
								bBlank = false;
							}
						}
						if ( i > 5 ) bBlank = false;
						if ( bBlank && !CarryRow ) sData = " ";

						if ( col.BackGroundColourDelegateFromRole != null )
							s += HtmlLib.TableDataAttr( 
                                sData, AttrFor( 
                                    dc, 
                                    col.BackGroundColourDelegateFromRole, 
                                    sVal ) );
						else
							s += HtmlLib.TableDataAttr( 
                                sData, 
                                AttrFor( 
                                    dc, 
                                    col.BackGroundColourDelegate, 
                                    sVal ) );
					}
					s += HtmlLib.TableRowClose();
				}
				s += TotalLine( tot );
				s += AverageLine( tot, rowCount );
				s += HtmlLib.TableClose();
			}
			return s;
		}

		//  To support StatGrid totaling
		private int QuantityOf( string strVal )
		{
			if ( Decimal.TryParse( strVal, out decimal qty ) ) return ( int ) qty;

			//  must be n:xx
			//  get first part
			var colonPart = strVal.IndexOf( ":", StringComparison.Ordinal );
			if ( colonPart < 0 )
				qty = 0;
			else
			{
				var firstPart = strVal.Substring( 0, colonPart );
				if ( !Decimal.TryParse( firstPart, out qty ) )
				{
					qty = 0;
				}
			}
			return ( int ) qty;
		}

		private string TotalLine( int[] tot )
		{
			var tl = "";

			if ( !Totals ) return "";

			if ( Body != null )
			{
				tl = HtmlLib.TableRowOpen();
				if ( DoRowNumbers )
					tl += HtmlLib.TableDataAttr( "Totals", "ALIGN='RIGHT' VALIGN='TOP'" );

				for ( int i = 0; i < _columns.Count; i++ )
				{
					var col = ( ReportColumn ) _columns[ i ];
					if ( col.CanAccumulate )
					{
						var dc = Body.Columns[ col.Source ];
						var sData = FormatData( dc, col.Format, tot[ i ].ToString( CultureInfo.InvariantCulture ) );
						tl += HtmlLib.TableDataAttr( sData, AttrFor( dc, ( ReportColumn.ColourDelegate ) null, "" ) );
						LastTotal = tot[ i ];
					}
					else if ( col.ColumnTotalDelegate != null )
					{
						string output = col.ColumnTotalDelegate();
						tl += HtmlLib.TableDataAttr( output, "ALIGN='CENTER' VALIGN='TOP'" );
					}
					else
						tl += HtmlLib.TableData( HtmlLib.HtmlPad( "", 1 ) );
				}
				tl += HtmlLib.TableRowClose();
			}
			return tl;
		}

		private string AverageLine( int[] tot, int rowCount )
		{
			var tl = "";

			if ( !Totals ) return "";

			if ( Body != null )
			{
				tl = HtmlLib.TableRowOpen();
				if ( DoRowNumbers )
					tl += HtmlLib.TableDataAttr( "Averages", "ALIGN='RIGHT' VALIGN='TOP'" );

				for ( int i = 0; i < _columns.Count; i++ )
				{
					var col = ( ReportColumn ) _columns[ i ];
					if ( col.CanAccumulate )
					{
						var dc = Body.Columns[ col.Source ];
						var sData = FormatData( dc, "{0:0.0}", string.Format( "{0:0.0}", ( ( decimal ) tot[ i ] / ( decimal ) rowCount ) ) );
						tl += HtmlLib.TableDataAttr( sData, AttrFor( dc, ( ReportColumn.ColourDelegate ) null, "" ) );
						LastTotal = tot[ i ];
					}
					else if ( col.ColumnTotalDelegate != null )
					{
						string output = col.ColumnTotalDelegate();
						tl += HtmlLib.TableDataAttr( output, "ALIGN='CENTER' VALIGN='TOP'" );
					}
					else
						tl += HtmlLib.TableData( HtmlLib.HtmlPad( "", 1 ) );
				}
				tl += HtmlLib.TableRowClose();
			}
			return tl;
		}

		private string ColHeaders()
		{
			var headers = "";
			if ( !string.IsNullOrEmpty( CustomHeader ) ) return CustomHeader;

			if ( _columns != null )
			{
				if ( _rowNumbers ) headers = HtmlLib.TableHeader( "Row" );

				headers = _columns.Cast<ReportColumn>().Aggregate( headers,
																  ( current, col ) => current + HtmlLib.TableHeader( col.Header ) );
			}
			return headers;
		}

		private static string AttrFor(
		   DataColumn dc,
		   ReportColumn.ColourDelegateFromRole bgColour,
		   string theValue )
		{
			var sAttr = "";
			if ( dc != null )
			{
				if ( dc.DataType == Type.GetType( "System.Decimal" ) ||
					dc.DataType == Type.GetType( "System.Int32" ) )
					sAttr = "ALIGN='RIGHT'";
				if ( dc.DataType != null && dc.DataType == Type.GetType( "System.String" ) )
					sAttr = "ALIGN='CENTER'";

				if ( bgColour != null )
				{
					if ( ( !string.IsNullOrEmpty( theValue ) ) )
						sAttr += " BGCOLOR=" + bgColour( theValue );
				}
			}
			return sAttr + " VALIGN='TOP'";
		}

		private static string AttrFor(
		   DataColumn dc,
		   ReportColumn.ColourDelegate bgColour,
		   string theValue )
		{
			var sAttr = "";
			if ( dc != null )
			{
				if ( dc.DataType == Type.GetType( "System.Decimal" ) ||
					dc.DataType == Type.GetType( "System.Int32" ) )
					sAttr = "ALIGN='RIGHT'";
				//sAttr = "class='num'";
				if ( dc.DataType != null && dc.DataType == Type.GetType( "System.String" ) )
					sAttr = "ALIGN='CENTER'";

				if ( bgColour != null )
				{
					if ( ( !string.IsNullOrEmpty( theValue ) ) )
					{
						//  look for data with a colon in it (we only need to use the number)
						var numberSpot = theValue.IndexOf( 
                            ":", 
                            StringComparison.Ordinal );
						if ( numberSpot > -1 )
						{
							var numberPart = theValue.Substring( 0, numberSpot );
							if ( numberPart.Equals( "-" ) )
								numberPart = theValue.Substring( 0, 2 );
							if ( !string.IsNullOrEmpty( numberPart ) )
								if ( !numberPart.Equals( ":" ) )
									sAttr += " BGCOLOR=" + bgColour( 
                                        Int32.Parse( numberPart ) );
						}
						else
						{
                            if ( theValue.Contains( '(' ) )
                            {
                                int rankNo = GetRankNo(theValue);
                                sAttr += " BGCOLOR=" + bgColour( rankNo  );
                            }
                            else
                            {
                                if ( Decimal.TryParse(
                                    theValue,
                                    out decimal parsedVal ) )
                                    sAttr += " BGCOLOR=" + bgColour( ( int ) parsedVal );
                                else
                                {
                                    var decimalString = GetInnerText( theValue );
                                    if ( Decimal.TryParse( decimalString, out decimal parsedStringVal ) )
                                        sAttr += " BGCOLOR=" + bgColour( ( int ) parsedStringVal );
                                }
                            }
						}
					}
				}
			}
			return sAttr + " VALIGN='TOP'";
		}

        private static int GetRankNo( string theValue )
        {
            var pattern = @"\((.*?)\)";
            var match = Regex.Match( theValue, pattern ).Value;
            match = match.Replace( '(', ' ' );
            match = match.Replace( ')', ' ' );
            return int.Parse( match );
        }

        private static string GetInnerText( string theValue )
		{
			var txt = System.Text.RegularExpressions.Regex.Replace( theValue, "(<[a|A][^>]*>|)", "" );
			return txt;
		}

		private static string GetNumbers( string input )
		{
			return new string( input.Where( c => char.IsDigit( c ) || c == '.' ).ToArray() );
		}

		private static string FormatData( DataColumn dc, string format, string data )
		{
			var sOut = data;

			if ( data != String.Empty )
			{
				if ( dc.DataType == Type.GetType( "System.Decimal" ) || dc.DataType == Type.GetType( "System.Int32" ) )
					sOut = Decimal.Parse( data ).Equals( -1 ) ? "-1" : string.Format( format, Decimal.Parse( data ) );
			}
			return sOut;
		}

		private static bool IsEven( int someNumber )
		{
			return someNumber == ( someNumber / 2 * 2 );
		}

		private string Header( string cHeading )
		{
			string htmlOut;

			htmlOut = HtmlLib.TableOpen( "class='title' cellpadding='0' cellspacing='0' width='100%'" ) + "\n\t"
							  + HtmlLib.TableRowOpen( TopLine() ) + "\n\t\t"
							  + HtmlLib.TableDataAttr( HtmlLib.Bold( cHeading ), "colspan='2' class='gponame'" ) + "\n\t"
							  + HtmlLib.TableRowClose() + "\n\t"
							  + HtmlLib.TableRowOpen() + "\n\t\t"
							  + HtmlLib.TableDataAttr( TopLine(), "id='dtstamp'" ) + "\n\t\t"
							  + HtmlLib.TableData( HtmlLib.Div( "objshowhide", "tabindex='0'" ) ) + "\n\t"
							  + HtmlLib.TableRowClose() + "\n"
							  + HtmlLib.TableClose() + "\n";

			return htmlOut;
		}

		private static string TopLine()
		{
			var theDate = string.Format( "Report Date: {0} ",
			   DateTime.Now.ToString( "dd MMM yy  HH:mm" ) );
			return theDate;
		}

		private string Footer()
		{
			var htmlOut = HtmlLib.TableOpen( "class='title' cellpadding='0' cellspacing='0'" ) + "\n\t"
							 + HtmlLib.TableRowOpen() + "\n\t\t"
							 + HtmlLib.TableDataAttr( _footer, "colspan='2' class='gponame'" ) + "\n\t"
							 + HtmlLib.TableRowClose() + "\n\t";
			if ( ShowElapsedTime )
			{
				htmlOut += HtmlLib.TableRowOpen() + "\n\t\t"
						   + HtmlLib.TableDataAttr( "elapsed time:" + TimeTaken, "id='dtstamp'" ) + "\n\t\t"
						   + HtmlLib.TableData( HtmlLib.Div( "objshowhide", "tabindex='0'" ) ) + "\n\t"
						   + HtmlLib.TableRowClose() + "\n";
			}
			htmlOut += HtmlLib.TableClose() + "\n";
			return htmlOut;
		}

		#endregion HTML Rendition

		#region CSV Rendition

		public void RenderAsCsv( string fileName, Logger logger )
		{
			var fileOut = string.Format( "{0}\\{2}\\Starters\\csv\\{1}.csv",
			   Utility.OutputDirectory(), fileName, Utility.CurrentSeason() );
			Utility.EnsureDirectory( fileOut );
			using ( var fs = File.Create( fileOut ) )
			using ( var sw = new StreamWriter( fs ) )
			{
				sw.WriteLine( CsvHeader() );
				foreach ( DataRow dr in _body.Rows )
					sw.WriteLine( CsvLine( dr ) );
			}
			logger.Info( $"{fileOut} rendered" );
		}

		private string CsvLine( DataRow dr )
		{
			var sb = new StringBuilder();
			foreach ( DataColumn col in _body.Columns )
				sb.Append( string.Format( "\"{0}\",", dr[ col.ColumnName ] ) );

			return sb.ToString();
		}

		private string CsvHeader()
		{
			var sb = new StringBuilder();
			foreach ( DataColumn col in _body.Columns )
				sb.Append( string.Format( "\"{0}\",", col.ColumnName ) );

			return sb.ToString();
		}

		#endregion CSV Rendition

		#region Excel rendition

		//public void RenderAsXls(string fileName)
		//{
		//   Excel.Application xlApp;
		//   Excel.Workbook xlWorkBook;
		//   Excel.Worksheet xlWorkSheet;
		//   object misValue = System.Reflection.Missing.Value;

		//   xlApp = new Excel.Application();
		//   xlWorkBook = xlApp.Workbooks.Add(misValue);
		//   xlWorkSheet = (Excel.Worksheet) xlWorkBook.Worksheets.get_Item(1);

		//   int column = 0;
		//   foreach (DataColumn col in _body.Columns)
		//   {
		//      column++;
		//      xlWorkSheet.Cells[1, column] = string.Format("{0}", col.ColumnName);
		//   }
		//   int row = 1;
		//   foreach (DataRow dr in _body.Rows)
		//   {
		//      row++;
		//      column = 0;

		//      foreach (DataColumn col in _body.Columns)
		//      {
		//         column++;
		//         xlWorkSheet.Cells[row, column] = string.Format("{0}", dr[col.ColumnName]);
		//      }
		//   }

		//   RosterLib.Utility.Announce(string.Format("cwd = {0}", ApplicationFolder()));

		//   xlWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookNormal,
		//                     misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive,
		//                     misValue, misValue, misValue, misValue, misValue);

		//   xlWorkBook.Close(true, misValue, misValue);
		//   xlApp.Quit();

		//   RosterLib.Utility.Announce(fileName + " created");

		//   releaseObject(xlWorkSheet);
		//   releaseObject(xlWorkBook);
		//   releaseObject(xlApp);
		//}

		public string ApplicationFolder()
		{
			return System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(
			   0,
			   System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf( "\\", StringComparison.Ordinal ) );
		}

		#endregion Excel rendition

		#endregion Output

		public void AddDenisStyle()
		{
			AddStyle(
			   "#container { text-align: left; background-color: #ccc; margin: 0 auto; border: 1px solid #545454; padding:10px; font: 13px/19px Trebuchet MS, Georgia, Times New Roman, serif; }" );
			AddStyle( "#main { margin-left:1em; }" );
			AddStyle( "#dtStamp { font-size:0.8em; }" );
			AddStyle( ".end { clear: both; }" );
			AddStyle( ".num { mso-number-format: General; }" );
			AddStyle( ".gponame { color:white; background:black }" );
			AddStyle(
			   "label { display:block; float:left; width:130px; padding: 3px 5px; margin: 0px 0px 5px 0px; text-align:right; }" );
			AddStyle(
			   "value { display:block; float:left; width:100px; padding: 3px 5px; margin: 0px 0px 5px 0px; text-align:left; font-weight: bold; color:blue }" );
			AddStyle(
			   "#notes { float:right; height:auto; width:308px; font-size: 88%; background-color: #ffffe1; border: 1px solid #666666; padding: 5px; margin: 0px 0px 10px 10px; color:#666666 }" );
			AddStyle(
			   "div.notes H4 { background-image: url(images/icon_info.gif); background-repeat: no-repeat; background-position: top left; padding: 3px 0px 3px 27px; border-width: 0px 0px 1px 0px; border-style: solid; border-color: #666666; color: #666666; font-size: 110%;}" );
		}
	}

	//end SimpleTableReport

	#region Helper classes

	public class ReportColumn
	{
		/// <summary>
		/// The name of the column which would appear in the column header.
		/// </summary>
		public string Header;

		/// <summary>
		/// The name of the field used to populate the column.
		/// </summary>
		public string Source;

		/// <summary>
		/// The formating template for the data.
		/// </summary>
		public string Format;

		public Type Type;

		public delegate string ColourDelegate( int colValue );

		public delegate string ColourDelegateFromRole( string colValue );

		public delegate string TotalDelegate();

		public ReportColumn( string header, string source, string format )
		{
			Header = header;
			Source = source;
			Format = format;
			CanAccumulate = false;
		}

		public ReportColumn( string header, string source, string format, ColourDelegateFromRole colourDelegateIn )
		{
			Header = header;
			Source = source;
			Format = format;
			CanAccumulate = false;
			BackGroundColourDelegateFromRole = colourDelegateIn;
		}

		public ReportColumn( string header, string source, string format, ColourDelegate colourDelegateIn )
		{
			Header = header;
			Source = source;
			Format = format;
			CanAccumulate = false;
			BackGroundColourDelegate = colourDelegateIn;
		}

		public ReportColumn( string header, string source, string format, Type type )
		{
			Header = header;
			Source = source;
			Format = format;
			Type = type;
			CanAccumulate = false;
		}

		public ReportColumn( string header, string source, string format, Type type, bool tally )
		{
			Header = header;
			Source = source;
			Format = format;
			Type = type;
			CanAccumulate = tally;
		}

		public ReportColumn( string header, string source, string format, Type type, bool tally,
		   ColourDelegate colourDelegateIn )
		{
			Header = header;
			Source = source;
			Format = format;
			Type = type;
			CanAccumulate = tally;
			BackGroundColourDelegate = colourDelegateIn;
		}

		public ReportColumn( string header, string source, string format, bool tally )
		{
			Header = header;
			Source = source;
			Format = format;
			CanAccumulate = tally;
		}

		public ReportColumn( string header, string source, string format, TotalDelegate totalDelegate )
		{
			Header = header;
			Source = source;
			Format = format;
			ColumnTotalDelegate = totalDelegate;
		}

		public bool CanAccumulate { get; set; }

		public TotalDelegate ColumnTotalDelegate { get; set; }

		public ColourDelegate BackGroundColourDelegate { get; set; }

		public ColourDelegateFromRole BackGroundColourDelegateFromRole { get; set; }
	}

	#endregion Helper classes
}

//end namespace 