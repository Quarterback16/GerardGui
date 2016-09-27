using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using Helpers;

namespace Gerard.Tests
{
   [TestClass]
   public class TestGamebookGetter
   {
      [TestMethod]
      public void TestGetGamebooksForCurrentWeek()
      {
         const string weekToDownload = "03";

         var week = new NFLWeek("2016", weekToDownload );
         var sut = new GamebookGetter( new Downloader( 
            string.Format( "g:\\tfl\\nfl\\gamebooks\\week {0}\\", weekToDownload ) ) );
         var result = sut.DownloadWeek(week);
         Assert.IsTrue(result > 0);
      }

      [TestMethod]
      public void TestOutputDirectory()
      {
         var week = new NFLWeek("2015", "03");
         var sut = new GamebookGetter(new Downloader("g:\\tfl\\nfl\\gamebooks\\week 03\\"));
         var result = sut.Downloader.OutputFolder;
         Assert.AreEqual(result, "g:\\tfl\\nfl\\gamebooks\\week 03\\" );
         Assert.IsTrue(System.IO.Directory.Exists(result));
      }

      [TestMethod]
      public void TestCurrentWeekSeed()
      {
         var week = new NFLWeek("2015", "01");
         var sut = new GamebookGetter(new Downloader());
         var result = sut.Seed(week);
         Assert.AreEqual(result, "56503");
      }
   }
}
