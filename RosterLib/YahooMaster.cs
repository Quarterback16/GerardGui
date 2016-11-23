using System.IO;
using System.Linq;
using System.Xml;
using RosterLib.Models;
using NLog;

namespace RosterLib
{
	public class YahooMaster : XmlCache
	{
		/// <summary>
		///   YahooMaster will keep a hash table of Yahoo Fantasy Point Output so they dont get re-created all the time.
		///   Key is season + week + PlayerId
		///     2012:01:AKERDA01  11
		/// </summary>
		public YahooMaster( string name, string fileName )
			: base( name )
		{
			Filename = string.Format( "{0}XML\\{1}", Utility.OutputDirectory(), fileName );
			LoadCache();
			IsDirty = false;  //  we r starting from the xml
		}

		private void LoadCache()
		{
			try
			{
				XmlDoc = new XmlDocument();
				XmlDoc.Load( Filename );
				var listNode = XmlDoc.ChildNodes[ 2 ];
				foreach ( XmlNode node in listNode.ChildNodes )
					AddXmlStat( node );
			}
			catch ( IOException e )
			{
				Logger.Error( string.Format( "Unable to open {1} xmlfile - {0}", e.Message, Filename ) );
			}
		}

		private void AddXmlStat( XmlNode node )
		{
			PutStat( new YahooOutput( node ) );
		}

		public override decimal GetStat( string theKey )
		{
			var season = theKey.Substring( 0, 4 );
			var week = theKey.Substring( 5, 2 );
			var playerId = theKey.Substring( 8, 8 );

			var stat = new YahooOutput
			{
				Season = season,
				Week = week,
				PlayerId = playerId,
				Quantity = 0.0M
			};

#if DEBUG
//			Utility.Announce( string.Format( "StatMaster:Getting Stat {0}", stat.FormatKey() ) );
#endif

			var key = stat.FormatKey();
			if ( TheHt.ContainsKey( key ) )
			{
				stat = (YahooOutput) TheHt[ key ];
				CacheHits++;
			}
			else
			{
				//  new it up
#if DEBUG
//				Utility.Announce( string.Format( "StatMaster:Instantiating Stat {0}", stat.FormatKey() ) );
#endif
				PutStat( stat );
				IsDirty = true;
				CacheMisses++;
			}
			return stat.Quantity;
		}

		public void PutStat( YahooOutput stat )
		{
			if ( stat.Quantity == 0.0M ) return;

			IsDirty = true;
			if ( TheHt.ContainsKey( stat.FormatKey() ) )
			{
				TheHt[ stat.FormatKey() ] = stat;
				return;
			}
			TheHt.Add( stat.FormatKey(), stat );
		}

      #region  Persistence

      public void Dump2Xml()
      {
         if ( ( TheHt.Count <= 0 ) || !IsDirty ) return;

         Utility.EnsureDirectory( Filename );  //  will create the dir if its not there

         var writer = new XmlTextWriter( Filename, null );

         writer.WriteStartDocument();
         writer.WriteComment( "Comments: " + Name );
         writer.WriteStartElement( "stat-list" );

         var myEnumerator = TheHt.GetEnumerator();
         while ( myEnumerator.MoveNext() )
         {
            var t = ( YahooOutput ) myEnumerator.Value;
            WriteStatNode( writer, t );
         }
         writer.WriteEndElement();
         writer.WriteEndDocument();
         writer.Close();
      }

      public void Dump2Xml(Logger logger)
		{
         Dump2Xml();
			Logger.Info( "  " + Filename + " written" );
		}

		private static void WriteStatNode( XmlWriter writer, YahooOutput stat )
		{
			writer.WriteStartElement( "stat" );
			writer.WriteAttributeString( "season", stat.Season );
			writer.WriteAttributeString( "week", stat.Week );
			writer.WriteAttributeString( "id", stat.PlayerId );
			writer.WriteAttributeString( "qty", stat.Quantity.ToString() );
			writer.WriteAttributeString( "opp", stat.Opponent );
			writer.WriteEndElement();
		}

		#endregion


		public void Calculate( string season, string week )
		{
			var theWeek = new NFLWeek( season, week );
			theWeek.LoadGameList();
			foreach ( var nflStat in theWeek.GameList().Cast<NFLGame>()
				.Select( game => game.GenerateYahooOutput() ).SelectMany( statList => statList ) )
				PutStat( nflStat );
		}

		public void Calculate( string season )
		{
			var theSeason = new NflSeason( season );
			theSeason.LoadRegularWeeksToDate();
			foreach ( var week in theSeason.RegularWeeks )
			{
#if DEBUG
				Utility.Announce( string.Format( "YahooMaster:Calculate Season {0} Week {1}", season, week.WeekNo ) );
#endif
				Calculate( theSeason.Year, week.Week );
			}
		}
	}


}

