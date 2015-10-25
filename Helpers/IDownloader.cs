using System;
namespace Helpers
{
   public interface IDownloader
   {
      bool Download(Uri target);
      bool DownloadPdf(Uri target);
      bool GotIt(Uri target);

      string OutputFolder { get; set;}
   }
}
