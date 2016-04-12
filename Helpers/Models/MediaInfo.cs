using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Helpers.Models
{
   public class MediaInfo
   {
      public FileInfo Info { get; set; }

      public string FileName { get; set; }

      public string PurifiedName { get; set; }

      public string Title { get; set; }

      public string Format { get; set; }

      public string TvWord { get; set; }

      public bool IsValid { get; set; }

      public bool IsTV { get; set; }

      public int Season { get; set; }

      public int Episode { get; set; }

      public bool IsMovie { get; set; }

      public bool IsNfl { get; set; }

      public bool IsSoccer { get; set; }

      public bool IsBook { get; set; }

      public bool IsMagazine { get; set; }

      public string MagazineFolder { get; set; }

      public List<string> Magazines { get; set; }

      public Logger Logger { get; set; }

      public string TvTitle { get; set; }

      public string MovieTitle { get; set; }

      public MediaInfo( string fileName )
      {
         try
         {
            if ( Logger == null ) Logger = LogManager.GetCurrentClassLogger();
            FileName = fileName;
            Info = new FileInfo( fileName );

            IsValid = true;
         }
         catch ( PathTooLongException ex )
         {
            Logger.Error( string.Format( "{0}:- Filename has {1} characters", ex.Message, FileName.Length
                ) );
            IsValid = false;
         }
         catch (Exception ex)
         {
            Logger.Error(string.Format("{0}:- Filename = {1} characters", ex.Message, FileName ));
            IsValid = false;
         }
      }

      public override string ToString()
      {
         return FileName;
      }

      public void Analyse()
      {
         if (Info == null) return;

         Title = Info.Name;

         if (Title.ToUpper().Contains("SAMPLE"))
            return;

         if (Title.ToUpper().Contains("TEST"))
            return;

         if (FileName.ToUpper().Contains("INPROGRESS"))
            return;

         IsNfl = DetermineNfl();

         Format = DetermineFormat();
         if ( !IsNfl )
         {
            IsBook = DetermineBook();
            if ( IsBook ) DetemineMagazine();

            Title = DetermineTitle();

            if (Title.ToUpper().Equals("JUDGE JUDY [2015]"))
            {
               Title = "Judge Judy";
               Season = 20;
               Episode = 1;
               IsTV = true;
            }
            else
            {
               if (!IsBook && !IsMagazine && !IsSoccer)
               {
                  Season = DetermineSeason();
                  if (Season > 0) Episode = DeterminEpisode();
                  if (Season > 0) IsTV = true;

                  IsMovie = DetermineMovie();

                  if (IsMovie)
                     FixTitle();
               }
            }
         }
         //OutputToConsole();
      }

      private void FixTitle()
      {
         //  quick and dirty way to purify the Title, based on the fac that it is the first part
         var newTitle = string.Empty;
         var lastChar = "x";

         for ( var i = 0; i < Title.Length; i++ )
         {
            var c = Title.Substring( i, 1 );
            if ( lastChar == " " && c == " " )
               break;
            lastChar = c;
            newTitle += c;
         }
         Title = newTitle.Trim();
      }

      private bool DetermineNfl()
      {
         if ( Title.ToUpper().Contains( "NFL" ) )
            return true;
         if ( Title.ToUpper().Contains( "PATRIOTS" ) )
            return true;
         if ( Title.ToUpper().Contains( "EAGLES" ) )
            return true;
         if ( Title.ToUpper().Contains( "SEAHAWKS" ) )
            return true;
         if ( Title.ToUpper().Contains( "DOLPHINS" ) )
            return true;
         if ( Title.ToUpper().Contains( "LIONS" ) )
            return true;
         if ( Title.ToUpper().Contains( "PANTHERS" ) )
            return true;
         if ( Title.ToUpper().Contains( "SAINTS" ) )
            return true;
         if ( Title.ToUpper().Contains( "RAMS" ) )
            return true;
         if ( Title.ToUpper().Contains( "BILLS" ) )
            return true;
         if ( Title.ToUpper().Contains( "RAVENS" ) )
            return true;
         if ( Title.ToUpper().Contains( "BRONCOS" ) )
            return true;
         if ( Title.ToUpper().Contains( "JAGUARS" ) )
            return true;
         if ( Title.ToUpper().Contains( "STEELERS" ) )
            return true;
         if ( Title.ToUpper().Contains( "COWBOYS" ) )
            return true;
         if ( Title.ToUpper().Contains( "49ERS" ) )
            return true;
         if ( Title.ToUpper().Contains( "CHARGERS" ) )
            return true;
         if ( Title.ToUpper().Contains( "TITANS" ) )
            return true;
         if ( Title.ToUpper().Contains( "CHIEFS" ) )
            return true;
         if ( Title.ToUpper().Contains( "COLTS" ) )
            return true;
         if ( Title.ToUpper().Contains( "BENGALS" ) )
            return true;
         if ( Title.ToUpper().Contains( "TEXANS" ) )
            return true;
         if ( Title.ToUpper().Contains( "BROWNS" ) )
            return true;
         if ( Title.ToUpper().Contains( "FALCONS" ) )
            return true;
         if ( Title.ToUpper().Contains( "GIANTS" ) )
            return true;
         if ( Title.ToUpper().Contains( "RAIDERS" ) )
            return true;
         if ( Title.ToUpper().Contains( "BEARS" ) )
            return true;
         if ( Title.ToUpper().Contains( "VIKINGS" ) )
            return true;
         if ( Title.ToUpper().Contains( "CARDINALS" ) )
            return true;
         if ( Title.ToUpper().Contains( "REDSKINS" ) )
            return true;
         if ( Title.ToUpper().Contains( "BUCCANEERS" ) )
            return true;
         if ( Title.ToUpper().Contains( "JETS" ) )
            return true;

         return false;
      }

      private void DetemineMagazine()
      {
         //  we need a list of magazines if we are to determine this
         if ( string.IsNullOrEmpty( MagazineFolder ) ) return;
         GetMagList();
         foreach ( var mag in Magazines )
         {
            if ( FileName.ToUpper().Contains( mag.ToUpper() ) )
            {
               IsMagazine = true;
               IsBook = false;
               break;
            }
         }
      }

      public void GetMagList()
      {
         if ( !Directory.Exists( MagazineFolder ) )
         {
            Logger.Error( string.Format( "Mag folder {0} does not exist", MagazineFolder ) );
            return;
         }
         try
         {
            Magazines = new List<string>();

            var fullDirs = Directory.GetDirectories( MagazineFolder );

            foreach ( var dir in fullDirs )
               Magazines.Add( dir.Substring( MagazineFolder.Length ) );

            Logger.Trace( string.Format( "Found {0} files in folder:{1}", Magazines.Count, MagazineFolder ) );
         }
         catch ( System.IO.IOException ex )
         {
            Logger.Error( string.Format( "Invalid Mag folder {0} :{1}", MagazineFolder, ex.Message ) );
         }
      }

      /// <summary>
      ///   Work out from the file details if it is a movie
      ///   A call to the XMl repository might be more
      ///   definitive as torrents are taged as Movie
      /// </summary>
      /// <returns></returns>
      private bool DetermineMovie()
      {
         if ( IsTV ) return false;
         if ( IsBook ) return false;
         if ( IsNfl ) return false;

         if (!Format.Equals( "MP4" ) && !Format.Equals( "AVI" ) && !Format.Equals( "MKV" )) return false;

         if ( Title.ToUpper().Contains( "SAMPLE" ) )
            return false;
         if ( Title.ToUpper().Contains( "JUDGE JUDY" ) )
            return false;
         if ( Title.ToUpper().Contains( "BBC" ) )
            return false;
         if ( Title.ToUpper().Contains( "CASTLE" ) )
            return false;
         return true;
      }

      private bool DetermineBook()
      {
         if ( Format == "PDF" ) return true;
         if ( Format == "ZIP" ) return true;
         if ( Format == "TGZ" ) return true;
         if ( Format == "CBZ" ) return true;
         if ( Format == "RAR" ) return true;
         if ( Format == "MOBI" ) return true;
         if ( Format == "EPUB" ) return true;
         if ( Format == "MB4" ) return true;
         return false;
      }

      private string DetermineFormat()
      {
         var theExt = Info.Extension;
         if ( theExt.Length > 0 )
            return Info.Extension.Substring( 1 ).ToUpper();
         return string.Empty;
      }

      private void OutputToConsole()
      {
         Console.WriteLine( "Title    : {0}", Title );
         Console.WriteLine( "Is Book  : {0}", IsBook );
         Console.WriteLine( "Is Mag   : {0}", IsMagazine );
         Console.WriteLine( "Is TV    : {0}", IsTV );
         Console.WriteLine( "Is Movie : {0}", IsMovie );
         Console.WriteLine( "Is NFL   : {0}", IsNfl );
         Console.WriteLine( "Is Soccer: {0}", IsSoccer );
         Console.WriteLine( "TV word  : {0}", TvWord );
         Console.WriteLine("Season   : {0}", Season);
         Console.WriteLine( "Episode  : {0}", Episode );
         Console.WriteLine( "Format   : {0}", Format );

         Logger.Trace( string.Format( "Title    : {0}", Title ) );
         Logger.Trace( string.Format( "Is Book  : {0}", IsBook ) );
         Logger.Trace( string.Format( "Is Mag   : {0}", IsMagazine ) );
         Logger.Trace( string.Format( "Is TV    : {0}", IsTV ) );
         Logger.Trace( string.Format( "Season   : {0}", Season ) );
         Logger.Trace( string.Format( "Episode  : {0}", Episode ) );
         Logger.Trace( string.Format( "Is Movie : {0}", IsMovie ) );
         Logger.Trace( string.Format( "Is NFL   : {0}", IsNfl ) );
         Logger.Trace( string.Format( "Is Soccer: {0}", IsSoccer ) );
         Logger.Trace( string.Format( "Format   : {0}", Format ) );
      }

      public string DetermineTitle()
      {
         var fileName = Info.Name;
         string title;
         CheckForSoccer( fileName, out title );
         if ( IsSoccer ) return title;

         if (fileName.ToUpper().Contains( "CASTLE" ))
            fileName = fileName.Replace( "2009", "" );
         if (fileName.ToUpper().Contains("FLASH"))
            fileName = fileName.Replace("2014", "");

         //Get rid of special noise characters
         fileName = fileName.Replace("_", " ");
         fileName = fileName.Replace("[", " [");
         PurifiedName = fileName;

         var regex = new Regex( @"\w*", RegexOptions.IgnoreCase );
         var matches = regex.Matches( fileName );
         foreach ( var match in matches )
         {
            var word = match.ToString();
            if ( word == string.Empty ) word = " ";

            if ( IsTvWord( word ) )
            {
               TvWord = word;
               break;
            }

            if ( IsYearWord( word ))
            {
               title += string.Format( "[{0}]", word );
               break;
            }
            if ( !String.Equals( word, Format, StringComparison.CurrentCultureIgnoreCase ) )
               title += word;
         }
         return title.Trim();
      }

      private void CheckForSoccer( string fileName, out string title )
      {
         title = string.Empty;
         if (!fileName.ToUpper().Contains( "EPL" ) && !fileName.ToUpper().Contains( "ARSENAL" ) &&
             !fileName.ToUpper().Contains("EURO") &&
             !fileName.ToUpper().Contains("JUVENTUS") && !fileName.ToUpper().Contains("HALF") &&
             !fileName.ToUpper().Contains( "SERIE A" )) return;
         IsSoccer = true;
         {
            title = fileName;
         }
      }

      private bool IsYearWord( string word )
      {
         if (!ContainsAllNumbers( word )) return false;
         if (word.Length != 4) return false;
         if (word.Substring( 0, 2 ) == "19" || word.Substring( 0, 2 ) == "20")
         {
            return true;
         }
         return false;
      }

      public bool IsTvWord( string word )
      {
         var regex = new Regex( @"\w*S[0-9][0-9]", RegexOptions.IgnoreCase );
         var matches = regex.Matches( word );
         if ( matches.Count > 0 ) return true;
         var regex2 = new Regex( @"\d+x\d+", RegexOptions.IgnoreCase );
         matches = regex2.Matches( word );
         if ( matches.Count > 0 ) return true;

         if ( ContainsAllNumbers( word ) )
         {
            if ( word.Length == 3 )
               return true;  //  eg 309
            if ( word.Length == 4 )
            {
               if ( ! IsYearWord(word) )
               {
                  return true;
               }
            }
         }
         return false;
      }

      private bool IsTitleWord( string word )
      {
         return !ContainsNumber( word );
      }

      public bool ContainsNumber( string word )
      {
         var containsNumber = Regex.IsMatch( word, @"[0-9]+", RegexOptions.IgnoreCase );
         return containsNumber;
      }

      public bool ContainsAllNumbers( string word )
      {
         var containsAllNumbers = Regex.IsMatch( word, @"^\d+$", RegexOptions.IgnoreCase );
         return containsAllNumbers;
      }

      private int DeterminEpisode()
      {
         var episode = 0;
         try
         {

            if (!string.IsNullOrEmpty(TvWord))
            {
               if (TvWord.Length == 3)
               {
                  return Int32.Parse(TvWord.Substring(1, 2));
               }
               else
               {
                  TvWord = TvWord.ToUpper();
                  if (TvWord.Contains("X"))
                  {
                     var e = TvWord.Substring(TvWord.IndexOf('X') + 1);
                     return Int32.Parse(e);
                  }
                  else
                  {
                     if ( TvWord.Length.Equals(4) )
                        return Int32.Parse(TvWord.Substring(2, 2));
                  }
               }
            }

            var regex = new Regex( @"\w*E\d+", RegexOptions.IgnoreCase );
            var matches = regex.Matches(PurifiedName);
            if ( matches.Count > 0 )
            {
               var candidateEpisode = matches[ 0 ].ToString().ToUpper();
               var eSpot = candidateEpisode.IndexOf( 'E' );
               var epString = candidateEpisode.Substring( eSpot + 1 );
               episode = Int32.Parse( epString );  // drop off the E
            }
         }
         catch ( FormatException ex )
         {
            Logger.Debug(string.Format("Could not determine Episode for {0} - {1}", PurifiedName, ex.Message));
         }

         return episode;
      }

      private int DetermineSeason()
      {

         int season = 0;

         try
         {
            if (!string.IsNullOrEmpty(TvWord))
            {
               if (TvWord.Length == 3)
               {
                  string firstChar = TvWord.Substring(0, 1);
                  Int32.TryParse(firstChar, out season);
                  return season;
               }
               else
               {
                  TvWord = TvWord.ToUpper();
                  if (TvWord.Contains("X"))
                  {
                     var s = TvWord.Substring(0, TvWord.IndexOf('X'));
                     Logger.Trace(string.Format("For  tvWord {0} season is {1}", TvWord, s));

                     return Int32.Parse(s);
                  }
                  else
                  {
                     if ( TvWord.Length == 4 )
                     {
                        return Int32.Parse(TvWord.Substring(0, 2));
                     }
                  }
               }
            }

            var regex = new Regex( @"\w*S[0-9][0-9]", RegexOptions.IgnoreCase );
            var matches = regex.Matches(PurifiedName);
            if ( matches.Count > 0 )
               season = Int32.Parse( matches[ 0 ].ToString().Substring( 1 ) );
         }
         catch ( Exception ex )
         {
            Logger.Debug(string.Format("Could not determine Season for {0} - {1}", PurifiedName, ex.Message));
            season = 0;
         }
         return season;
      }

      public bool HasValidExt()
      {
         var isValid = false;
         if ( Info.Extension.ToUpper().Equals( ".MP4" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".AVI" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".MKV" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".FLV" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".PDF" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".EPUB" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".MOBI" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".MB4" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".CBZ" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".RAR" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".TGZ" ) )
            isValid = true;
         else if ( Info.Extension.ToUpper().Equals( ".CBZ" ) )
            isValid = true;
         else if (Info.Extension.ToUpper().Equals(".AZW3"))
            isValid = true;
         return isValid;
      }
   }
}