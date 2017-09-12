using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class GamebookGetterTests
	{
		[TestMethod]
		public void TestGetGamebooksForCurrentWeek()
		{
			const string weekToDownload = "01";

			var week = new NFLWeek( "2017", weekToDownload );
			var sut = new GamebookGetter( new Downloader(
			   string.Format( "e:\\tfl\\nfl\\gamebooks\\week {0}\\", weekToDownload ) ) );
			var result = sut.DownloadWeek( week );
			Assert.IsTrue( result > 0 );
		}

		[TestMethod]
		public void TestDownloadSinglePdf()
		{
			const string weekToDownload = "01";

			var week = new NFLWeek( "2017", weekToDownload );
			var sut = new Downloader(
			   string.Format( "e:\\tfl\\nfl\\gamebooks\\week {0}\\", weekToDownload ) );
			var uri = new Uri( "http://www.nfl.com/liveupdate/gamecenter/57238/CLE_Gamebook.pdf" );
			var result = sut.Download( uri );
			Assert.IsTrue( result );
		}

		[TestMethod]
		public void TestOutputDirectory()
		{
			var week = new NFLWeek( "2015", "03" );
			var sut = new GamebookGetter( new Downloader( "g:\\tfl\\nfl\\gamebooks\\week 03\\" ) );
			var result = sut.Downloader.OutputFolder;
			Assert.AreEqual( result, "g:\\tfl\\nfl\\gamebooks\\week 03\\" );
			Assert.IsTrue( System.IO.Directory.Exists( result ) );
		}

		[TestMethod]
		public void TestCurrentWeekSeed()
		{
			var week = new NFLWeek( "2015", "01" );
			var sut = new GamebookGetter( new Downloader() );
			var result = sut.Seed( week );
			Assert.AreEqual( result, "56503" );
		}
	}
}