using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;

namespace RosterLib
{
	/// <summary>
	/// Summary description for NFLDivision.
	/// </summary>
	public class NFLDivision
	{
		/// <summary>
      ///   Loads a Conference with the teams
      /// </summary>
      /// <param name="nameIn"></param>
      /// <param name="confIn"></param>
      /// <param name="codeIn"></param>
      /// <param name="seasonIn"></param>
      /// <param name="catIn"></param>
		public NFLDivision( string nameIn, string confIn, string codeIn, string seasonIn, string catIn )
		{
#if DEBUG
         Utility.Announce( string.Format( "NFLDivision Constructing Division {0} cat {1}", nameIn, catIn ) );
#endif
			Name = nameIn;
			Conference = confIn;
			Code = codeIn;
			Season = seasonIn;
			Category = catIn;
			TeamList = new ArrayList();
			LoadTeams();
         TeamList.Sort();  //  into what order? - Clip according to the CompareTo in NFLTeam
		}
		
		public NFLDivision( string nameIn, string confIn, string codeIn, string seasonIn )
		{
#if DEBUG
			Utility.Announce( "Constructing Quick Division " + nameIn );
#endif
			Name = nameIn;
			Conference = confIn;
			Code = codeIn;
			Season = seasonIn;
			TeamList = new ArrayList();
			LoadTeams();
			TeamList.Sort();
		}

		
      private void LoadTeams()
		{
#if DEBUG
         Utility.Announce(string.Format("NFlDivision:LoadTeams: Load teams for {0} code {1}", Season, Code ));
#endif
			var strFilter = Filters.TeamFilter();

			var ds = Utility.TflWs.GetTeams( Season, Code );
			var dt = ds.Tables["teams"];
			foreach (DataRow dr in dt.Rows)
			{
				var strTeamCode = dr["teamid"].ToString();
#if DEBUG            
				Utility.Announce(string.Format("NFLDivision.LoadTeams: Loading {0} Clip= {1}",
				                                    dr["teamname"], dr["clip"] ) );
#endif
				var nIndex = strFilter.IndexOf( strTeamCode );
				if (nIndex <= -1) continue;
				var t = Masters.Tm.GetTeam( Season, strTeamCode );
				t.CurrentClip = Decimal.Parse( dr[ "clip" ].ToString() );  //  clip is not in the XML
				//t.LoadLineupPlayers();  // lazy load this
				TeamList.Add( t );
			}
		}
		
      public string NameOut()
		{
			return Conference + " - " + Name;
		}

      public static string ShortNameOut( string divCode )
      {
         string divOut;
         switch ( divCode )
         {
            case "A":
               divOut = "NE";
               break;
            case "B":
               divOut = "NN";
               break;
            case "C":
               divOut = "NS";
               break;
            case "D":
               divOut = "NW";
               break;
            case "E":
               divOut = "AE";
               break;
            case "F":
               divOut = "AN";
               break;
            case "G":
               divOut = "AS";
               break;
            case "H":
               divOut = "AW";
               break;
            default:
               divOut = "??";
               break;
         }
         return divOut;         
      }
		
      public string DivHtml( bool showBackups, string catIn, string strPos  )
		{
			var s = HtmlLib.TableOpen("BORDER=1 BORDERCOLOR=BLACK BGCOLOR=WHITE") + "\n" +
				HtmlLib.TableRowOpen() + "\n";
      	s = TeamList.Cast< NflTeam >().Aggregate( s, ( current, t ) => current + t.BoxHtml( showBackups, catIn, strPos ) );

      	return s + HtmlLib.TableRowClose() + "\n" + HtmlLib.TableClose() + "\n";
		}
		
      public string DivInjuries()
		{
			var s = HtmlLib.TableOpen("BORDER=1 BORDERCOLOR=BLACK BGCOLOR=WHITE") + "\n" +
				HtmlLib.TableRowOpen() + "\n";
			//foreach ( NflTeam t in TeamList )
			//   s += t.InjuryHtml();		
      	s = TeamList.Cast< NflTeam >().Aggregate( s, ( current, t ) => current + t.InjuryHtml() );

      	return s + HtmlLib.TableRowClose() + "\n" + HtmlLib.TableClose() + "\n";
		}
	
      public string DivDiv()
		{
			var d = new DivBlock( Name, 1, "he1_expanded" );
			d.AddContainer( TeamListDiv() );
			return d.Html();
		}
		
      public string TeamListDiv()
		{
         var sb = new StringBuilder();
         foreach (NflTeam t in TeamList)
				sb.Append( t.TeamDiv() );		 
      		
			return sb.ToString();
		}

      public void TeamCards()
      {
      	foreach ( NflTeam t in TeamList )
            t.RenderTeamCard();		 
      }

      public string Kickers()
      {
         //  Start off with the division name
         var s = HtmlLib.TableRowOpen( "BGCOLOR='LIGHTGREY'" ) + 
            HtmlLib.TableDataAttr( HtmlLib.Font( "VERDANA", NameOut(), "-1" ), "ALIGN='CENTER' COLSPAN='19'" ) +
            HtmlLib.TableRowClose() + "\n";
      	foreach ( NflTeam t in TeamList )
			{
            s += t.KickerProjection();
		      FieldGoals += t.FieldGoals;
         }
         return s;
      }

      public string SeasonProjection( string metricName, IPrognosticate predictor, DateTime projectionDate )
      {
         //  Start off with the division name
         var s = HtmlLib.TableRowOpen( "BGCOLOR='LIGHTGREY'" ) + 
            HtmlLib.TableDataAttr( HtmlLib.Font( "VERDANA", NameOut(), "-1" ), "ALIGN='CENTER' COLSPAN='19'" ) +
            HtmlLib.TableRowClose() + "\n";
      	return TeamList.Cast< NflTeam >()
				.Aggregate( s, ( current, t ) => current + t.SeasonProjection( predictor, metricName, projectionDate ) );
      }

      #region  Accessors

		public int FieldGoals { get; set; }

		public string Name { get; set; }

		public string Conference { get; set; }

		public string Code { get; set; }

		public string Category { get; set; }

		public string Season { get; set; }

		public ArrayList TeamList { get; set; }

		#endregion
   }
}
