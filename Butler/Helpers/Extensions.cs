using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Butler.Helpers
{
   public static class Extensions
   {
      public static void Sort<T>( this List<T> list, string sortExpression )
      {
         var sortExpressions = sortExpression.Split( new string[] { "," }, StringSplitOptions.RemoveEmptyEntries );

         var comparers = new List<GenericComparer>();

         foreach ( string sortExpress in sortExpressions )
         {
            var sortProperty = sortExpress.Trim().Split( ' ' )[ 0 ].Trim();
            var sortDirection = sortExpress.Trim().Split( ' ' )[ 1 ].Trim();

            var type = typeof( T );
            var PropertyInfo = type.GetProperty( sortProperty );
            if ( PropertyInfo == null )
            {
               var props = type.GetProperties();
               foreach ( PropertyInfo info in props )
               {
                  if ( info.Name.ToString().ToLower() == sortProperty.ToLower() )
                  {
                     PropertyInfo = info;
                     break;
                  }
               }
               if ( PropertyInfo == null )
               {
                  throw new Exception( String.Format( "{0} is not a valid property of type: \"{1}\"", sortProperty, type.Name ) );
               }
            }

            var sortDir = SortDirection.Ascending;
            if ( sortDirection.ToLower() == "asc" || sortDirection.ToLower() == "ascending" )
            {
               sortDir = SortDirection.Ascending;
            }
            else if ( sortDirection.ToLower() == "desc" || sortDirection.ToLower() == "descending" )
            {
               sortDir = SortDirection.Descending;
            }
            else
            {
               throw new Exception( "Valid SortDirections are: asc, ascending, desc and descending" );
            }

            comparers.Add( new GenericComparer { SortDirection = sortDir, PropertyInfo = PropertyInfo, comparers = comparers } );
         }
         list.Sort( comparers[ 0 ].Compare );
      }
   }

   public static class StringExtension
   {
      public static string Last( this string source, int tail_length )
      {
         if ( tail_length >= source.Length )
            return source;
         return source.Substring( source.Length - tail_length );
      }
   }

   public class GenericComparer
   {
      public List<GenericComparer> comparers { get; set; }
      int level = 0;

      public SortDirection SortDirection { get; set; }
      public PropertyInfo PropertyInfo { get; set; }

      public int Compare<T>( T t1, T t2 )
      {
         var ret = 0;

         if ( level >= comparers.Count )
            return 0;

         var t1Value = comparers[ level ].PropertyInfo.GetValue( t1, null );
         var t2Value = comparers[ level ].PropertyInfo.GetValue( t2, null );

         if ( t1 == null || t1Value == null )
         {
            if ( t2 == null || t2Value == null )
            {
               ret = 0;
            }
            else
            {
               ret = -1;
            }
         }
         else
         {
            if ( t2 == null || t2Value == null )
            {
               ret = 1;
            }
            else
            {
               ret = ( ( IComparable ) t1Value ).CompareTo( ( ( IComparable ) t2Value ) );
            }
         }
         if ( ret == 0 )
         {
            level += 1;
            ret = Compare( t1, t2 );
            level -= 1;
         }
         else
         {
            if ( comparers[ level ].SortDirection == SortDirection.Descending )
            {
               ret *= -1;
            }
         }
         return ret;
      }
   }

   public class ExampleUser
   {
      public DateTime Birthday { get; set; }
      public string Firstname { get; set; }
   }

   public class ExampleClass
   {
      public static void Example()
		{
			var userlist = new List<ExampleUser>();
			userlist.Add( new ExampleUser { Birthday = new DateTime( 1988, 10, 1 ), Firstname = "Bryan" } );
			userlist.Add( new ExampleUser { Birthday = new DateTime( 1986, 11, 4 ), Firstname = "Michael" } );
			userlist.Add( new ExampleUser { Birthday = new DateTime( 1977, 2, 2 ), Firstname = "Arjan" } );
			userlist.Add( new ExampleUser { Birthday = new DateTime( 1990, 6, 13 ), Firstname = "Pieter" } );
			userlist.Add( new ExampleUser { Birthday = new DateTime( 1988, 10, 1 ), Firstname = "Ruben" } );
			userlist.Add( new ExampleUser { Birthday = new DateTime( 1987, 8, 21 ), Firstname = "Bastiaan" } );
			userlist.Add( new ExampleUser { Birthday = new DateTime( 1987, 8, 21 ), Firstname = "Pieter" } );

			var unsorted = "Unsorted: " + Environment.NewLine;
			var builder = new System.Text.StringBuilder();
			builder.Append( unsorted );
			foreach ( ExampleUser user in userlist )
			{
				builder.Append( String.Format( "{0} / {1} {2}", user.Birthday.ToString( "dd-MM-yyyy" ), user.Firstname, Environment.NewLine ) );
			}
			unsorted = builder.ToString();

			userlist.Sort( "Firstname asc" );
			var sorted1 = "Sorted by Firstname ascending: " + Environment.NewLine;
			var builder1 = new System.Text.StringBuilder();
			builder1.Append( sorted1 );
			foreach ( ExampleUser user in userlist )
			{
				builder1.Append( String.Format( "{0} / {1} {2}", user.Birthday.ToString( "dd-MM-yyyy" ), user.Firstname, Environment.NewLine ) );
			}
			sorted1 = builder1.ToString();

			userlist.Sort( "Firstname asc, Birthday desc" );
			var sorted2 = "Sorted by Firstname ascending, Birtday descending: " + Environment.NewLine;
			var builder2 = new System.Text.StringBuilder();
			builder2.Append( sorted2 );
			foreach ( ExampleUser user in userlist )
			{
				builder2.Append( String.Format( "{0} / {1} {2}", user.Birthday.ToString( "dd-MM-yyyy" ), user.Firstname, Environment.NewLine ) );
			}
			sorted2 = builder2.ToString();

			userlist.Sort( "Birthday asc, Firstname asc" );
			var sorted3 = "Sorted by Birthday ascending, Firstname ascending: " + Environment.NewLine;
			var builder3 = new System.Text.StringBuilder();
			builder3.Append( sorted3 );
			foreach ( ExampleUser user in userlist )
			{
				builder3.Append( String.Format( "{0} / {1} {2}", user.Birthday.ToString( "dd-MM-yyyy" ), user.Firstname, Environment.NewLine ) );
			}
			sorted3 = builder3.ToString();
		}
	}
}
