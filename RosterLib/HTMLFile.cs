using System;
using System.IO;
using System.Collections;


namespace RosterLib
{
	/// <summary>
	/// Summary description for HTMLFile.
	/// </summary>
	public class HtmlFile
	{
		private string _filename;
		private readonly string _title;
		private readonly string _header;
		private readonly string _cssFile;
		private readonly string _script1;
		private readonly string _script2;
		private readonly ArrayList _bodyList;

		public bool AnnounceIt { get; set; }
		
		public HtmlFile(string filenameIn, string titleIn )
		{
			_filename = filenameIn;
			_title = titleIn;
			_header = String.Empty;
			_bodyList = new ArrayList();
			StyleList = new ArrayList();
			AnnounceIt = true;
		}

		public HtmlFile( string filenameIn, string titleIn, string headerIn, 
							  string cssFileIn, string script1In, string script2In )
		{
			_filename = filenameIn;
			_title = titleIn;
			_header = headerIn;
			_cssFile = cssFileIn;
			_script1 = script1In;
			_script2 = script2In;
			_bodyList = new ArrayList();
			StyleList = new ArrayList();
			AnnounceIt = true;
		}

		public ArrayList StyleList { get; set; }

		public void AddToBody( string strBody )
		{
			_bodyList.Add(strBody);
		}

		public void Render( string newFile )
		{
			_filename = newFile;
			Render();
		}

		public void Render()
		{
			Utility.EnsureDirectory(_filename);
			var sw = new StreamWriter(_filename,false);
			sw.WriteLine(_header.Length > 0 ? HtmlLib.HTMLOpenPlus(_header) : HtmlLib.HTMLOpen());
			sw.WriteLine( HtmlLib.HeadOpen()  );
			sw.WriteLine( "\t" + HtmlLib.HTMLTitle( _title ) );
			if ( _cssFile != null )
			{
				if ( _cssFile.Length > 0 )
					sw.WriteLine( "\t" + HtmlLib.CssLink( _cssFile ) );
			}
			if ( _script1 != null )
			{
				if ( _script1.Length > 0 )
					sw.WriteLine( "\t" + HtmlLib.VBScriptFile( _script1 ) );
			}
			if ( _script2 != null )
			{
				if ( _script2.Length > 0 )
					sw.WriteLine( "\t" + HtmlLib.JSScriptFile( _script2 ) );
			}
			
			if ( StyleList.Count > 0 )
			{
				var styleEnumerator = StyleList.GetEnumerator();
				sw.WriteLine( "\t" + HtmlLib.StyleOpen() );
				while ( styleEnumerator.MoveNext() )				
					sw.WriteLine( "\t" + styleEnumerator.Current );
				sw.WriteLine( "\t" + HtmlLib.StyleClose() );				
			}

			sw.WriteLine( HtmlLib.HeadClose()  );
			sw.WriteLine( HtmlLib.BodyOpen() );

			sw.WriteLine( HtmlLib.DivOpen( "id=\"container\"") );

			//  Add the body parts
			var myEnumerator = _bodyList.GetEnumerator();
			while ( myEnumerator.MoveNext() )
				sw.WriteLine( myEnumerator.Current );

			sw.WriteLine( HtmlLib.DivClose() );
			
			sw.WriteLine( HtmlLib.BodyClose() );
			sw.WriteLine( HtmlLib.HtmlClose() );
			sw.Close();

			if ( AnnounceIt )
			   Utility.Announce( string.Format( "   {0} has been rendered", _filename ) );
		}
		
		public void AddStyle( string style )
		{
			StyleList.Add( style );
		}
	}
}
