using System;
using System.Collections;
using System.Text;

namespace RosterLib
{
	/// <summary>
	/// Summary description for NFLConference.
	/// </summary>
	public class NflConference
	{
		public NflConference( string confIn, string seasonIn )
		{
			FieldGoals = 0;
			Conference = confIn;
			Season = seasonIn;
			DivList = new ArrayList();
		}

		public void AddDiv(string strDivName, string strDivCode )
		{
#if DEBUG
			Utility.Announce( string.Format( "NFLConference.AddDiv: {0} Adding {1}", Conference, strDivName ) );
#endif
			var div = new NFLDivision( strDivName, Conference, strDivCode, Season, "" );
			DivList.Add( div );
		}
		
		public void QuickAddDiv(string strDivName, string strDivCode )
		{
#if DEBUG
         Utility.Announce(string.Format("NFLConference.QuickAddDiv: {0} Quick Adding {1}", Conference, strDivName));
#endif
			var div = new NFLDivision( strDivName, Conference, strDivCode, Season );
			DivList.Add( div );
		}

		public string NameOut()
		{
			return Conference;
		}

		public string ConfHtml()
		{
			var d = new DivBlock( NameOut(), 0, "he0_expanded" );
			d.AddContainer( DivisionList() );
			return d.Html();
		}

		public string DivisionList()
		{
		   var sb = new StringBuilder();
			foreach ( NFLDivision d in DivList )
				sb.Append( d.DivDiv() );
			return sb.ToString();
		}

      public void TeamCards()
      {
         //  for each division, render TeamCards
         var myEnumerator = DivList.GetEnumerator();
         while ( myEnumerator.MoveNext() )
         {
            var d = (NFLDivision) myEnumerator.Current;
            d.TeamCards();				 
         }
      }

      public string Kickers()
      {
         //  for each division, render Kicker projections
         var s = String.Empty;
         var myEnumerator = DivList.GetEnumerator();
         while ( myEnumerator.MoveNext() )
         {
            var d = (NFLDivision) myEnumerator.Current;
            s += d.Kickers();
				FieldGoals += d.FieldGoals; 
         }
         return s;
      }

      public string SeasonProjection(string metricName, IPrognosticate predictor, DateTime projectionDate )
      {
         //  for each division, render projections
         var sb = new StringBuilder();
      	foreach ( NFLDivision d in DivList )
      		sb.Append( d.SeasonProjection( metricName, predictor, projectionDate ) );
      	return sb.ToString();
      }

		public int FieldGoals { get; set; }

		public string Conference { get; set; }

		public string Season { get; set; }

		public ArrayList DivList { get; set; }
	}
}
