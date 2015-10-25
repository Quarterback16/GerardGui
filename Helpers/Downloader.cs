using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using NLog;

namespace Helpers
{
    public class Downloader : Helpers.IDownloader
    {
        public Logger Logger { get; set; }

        public string OutputFolder { get; set; }

        public Downloader()
        {
            Logger = LogManager.GetCurrentClassLogger();
            OutputFolder = ".//Source//";
        }

        public Downloader(string outputFolder )
        {
           Logger = LogManager.GetCurrentClassLogger();
           OutputFolder = outputFolder;
        }

        public bool GotIt( Uri target )
        {
            var gotIt = false;
            var fileName = GetFileName(target);
            var fullFile = TackOnOutputDirectory(fileName);
            if (File.Exists(fullFile))
            {
               Logger.Trace(string.Format("   Already got {0}", fullFile));
               gotIt = true;
            }
            return gotIt;
        }

        public static string GetFileName(Uri target)
        {
            var fileName = Path.GetFileName(target.LocalPath);
            return fileName;
        }

        public bool Download(Uri target )
        {
            if (GotIt(target)) return false;  //  we already seen this file

				var downloaded = false;

            var fileName = GetFileName(target);

            fileName = TackOnOutputDirectory(fileName);
            try
            {
                byte[] result;
                byte[] buffer = new byte[4096];

                WebRequest wr = WebRequest.Create(target);
                wr.ContentType = "application/x-bittorrent";
                using (WebResponse response = wr.GetResponse())
                {
                    var gzip = response.Headers["Content-Encoding"] == "gzip";
                    var responseStream = gzip
                                            ? new GZipStream(response.GetResponseStream(), CompressionMode.Decompress)
                                            : response.GetResponseStream();

                    using (var memoryStream = new MemoryStream())
                    {
                        int count;
                        do
                        {
                            count = responseStream.Read(buffer, 0, buffer.Length);
                            memoryStream.Write(buffer, 0, count);
                        } while (count != 0);

                        result = memoryStream.ToArray();

                        using (var writer = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
                        {
                            writer.Write(result);
                        }
                    }
                }
					 downloaded = true;
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("There was a problem downloading the file - {0}", ex.Message));
            }
            return downloaded;
        }

        public bool DownloadPdf(Uri target)
        {
           if (GotIt(target)) return false;  //  we already seen this file

           var downloaded = false;

           var fileName = GetFileName(target);

           fileName = TackOnOutputDirectory(fileName);
           try
           {
              byte[] result;
              byte[] buffer = new byte[4096];

              WebRequest wr = WebRequest.Create(target);
              //wr.ContentType = "application/x-bittorrent";
              using (WebResponse response = wr.GetResponse())
              {
                 var gzip = response.Headers["Content-Encoding"] == "gzip";
                 var responseStream = gzip
                                         ? new GZipStream(response.GetResponseStream(), CompressionMode.Decompress)
                                         : response.GetResponseStream();

                 using (var memoryStream = new MemoryStream())
                 {
                    int count;
                    do
                    {
                       count = responseStream.Read(buffer, 0, buffer.Length);
                       memoryStream.Write(buffer, 0, count);
                    } while (count != 0);

                    result = memoryStream.ToArray();

                    using (var writer = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
                    {
                       writer.Write(result);
                    }
                 }
              }
              downloaded = true;
           }
           catch (Exception ex)
           {
              Logger.Error(string.Format("There was a problem downloading the file - {0}", ex.Message));
           }
           return downloaded;
        }

        private string TackOnOutputDirectory(string fileName)
        {
            return string.Format("{0}{1}", OutputFolder, fileName );
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}