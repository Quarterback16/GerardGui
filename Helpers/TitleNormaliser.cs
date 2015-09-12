using Helpers.Interfaces;
using Helpers.Models;
using NLog;
using System.IO;

namespace Helpers
{
   public class TitleNormaliser : INormaliseTitles
   {
      public Logger Logger { get; set; }

      public TitleNormaliser()
      {
         Logger = LogManager.GetCurrentClassLogger();
      }

      public string NormaliseTitle(string title, string type)
      {
//       Logger.Debug("Title IN:{0}", title);
         title = title.Replace("_", " ");
         title = title.Replace("\"", "'");
         title = title.Replace(".", " ");
         title = title.Replace(":", " ");
         title = title.Replace("|", " ");
         title = title.Replace("/", " ");
         title = title.Replace("\\", " ");
         title = title.Replace("*", " ");
         title = title.Replace("?", " ");
         title = title.Replace(">", " ");
         title = title.Replace("<", " ");  //  this may put a lot of spaces in the title
         var newTitle = string.Empty;
         for (int i = 0; i < title.Length; i++)
         {
            var aLetter = title.Substring(i, 1);
            if (i > 0)
            {
               if (aLetter == " ")
               {
                  if (title.Substring(i - 1, 1).Equals(" "))
                  {
                     continue;
                  }
               }
            }
            newTitle += aLetter;
         }
         title = newTitle;
         if (type.Equals("TV"))
         {
            var tvtitle = TvTitle(title);
            if (!string.IsNullOrEmpty(tvtitle))
               title = tvtitle;
         }
         else if (type.Equals("Movies"))
         {
            var movietitle = MovieTitle(title);
            if (!string.IsNullOrEmpty(movietitle))
               title = movietitle;
         }
//       Logger.Debug("Title OT:{0}", title.ToUpper());
         return title.ToUpper();
      }

      private string TvTitle(string title)
      {
         Logger.Trace("Extracting TV title from " + title);
         var mi = new MediaInfo(title);
         mi.Analyse();
         Logger.Debug("TV title is " + mi.TvTitle);

         return mi.TvTitle;
      }

      private string MovieTitle(string title)
      {
          var theTitle = string.Empty;
          try
          {
              Logger.Debug("Extracting Movie title from " + title);
              var mi = new MediaInfo(title);
              mi.Analyse();
              Logger.Debug("   Movie title is " + mi.Title);
              theTitle = mi.Title;
          }
          catch (PathTooLongException ex)
          {
              Logger.Error(string.Format("{0}:- Title has {1} characters", ex.Message, title.Length
                  ));
              theTitle = title.Substring(0, 100);
          }
          catch (System.Exception)
          {
              
              throw;
          }

          return theTitle;
      }
   }
}