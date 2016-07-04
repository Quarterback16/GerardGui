using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gerard.Tests
{
   [TestClass]
   public class DownloadTests
   {
      //http://www.nfl.com/liveupdate/gamecenter/56505/CHI_Gamebook.pdf
      [TestMethod]
      public void TestDownloadGamebook()
      {
         var sut = new Downloader();
         var uri = new Uri("http://www.nfl.com/liveupdate/gamecenter/56505/CHI_Gamebook.pdf");
         Assert.IsTrue( sut.DownloadPdf( uri ) );
      }

      [TestMethod]
      public void TestDownloadWeeksGamebooks()
      {
         var sut = new Downloader();

         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56503/NE_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56504/BUF_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56505/CHI_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56506/HOU_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56507/JAX_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56508/NYJ_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56509/STL_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56510/WAS_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56511/ARI_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56512/SD_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56513/DEN_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56514/OAK_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56515/TB_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56516/DAL_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56517/ATL_Gamebook.pdf"));
         //sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56518/SF_Gamebook.pdf"));

         sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56519/KC_Gamebook.pdf"));
         sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56520/BUF_Gamebook.pdf"));
         sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56521/CAR_Gamebook.pdf"));
         sut.DownloadPdf(new Uri("http://www.nfl.com/liveupdate/gamecenter/56522/CHI_Gamebook.pdf"));
      }
   }
}
