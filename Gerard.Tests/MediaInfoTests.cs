using Helpers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace Gerard.Tests
{
    [TestClass]
    public class MediaInfoTests
    {
        #region Movie Tests

        [TestMethod]
        public void TestMediaInfo()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Mean Machine.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsTrue(mi.IsMovie);
            }
        }

        [TestMethod]
        public void TestMediaInfoTvSample()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Sliders.S02E04.Sample.avi", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }

        [TestMethod]
        public void TestMediaInfoSample2()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Sample.mkv", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }

        [TestMethod]
        public void TestMediaInfoHalf()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "1st half.mkv", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }


        [TestMethod]
        public void TestMediaInfoMovie()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "American.Sniper.2014.720p.BRRip.XviD.AC3.SANTi.avi", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsFalse(mi.IsTV);
                Assert.IsTrue(mi.IsMovie);
                Assert.IsTrue(mi.Title.Equals("American Sniper [2014]"));
            }
        }

        [TestMethod]
        public void Test12MonkeysMovie()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "12.Monkeys.1995.BluRay.x264.720p .YIFY.mp4", SearchOption.AllDirectories).ToList();

            foreach (var mi in testCandidates.Select(item => new MediaInfo(item)))
            {
                mi.Analyse();
                Assert.IsFalse(mi.IsTV);
                Assert.IsTrue(mi.IsMovie);
                Assert.IsTrue(mi.Title.Equals("12 Monkeys [1995]"));
            }
        }

        [TestMethod]
        public void TestMeanMachineMovie()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Mean Machine.mp4", SearchOption.AllDirectories).ToList();

            foreach (var mi in testCandidates.Select(item => new MediaInfo(item)))
            {
                mi.Analyse();
                Assert.IsFalse(mi.IsTV);
                Assert.IsTrue(mi.IsMovie);
                Assert.IsTrue(mi.Title.Equals("Mean Machine"));
            }
        }

        [TestMethod]
        public void TestMediaInfoSample()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "American.Sniper.sample.avi", SearchOption.AllDirectories).ToList();

            foreach (var mi in testCandidates.Select(item => new MediaInfo(item)))
            {
                mi.Analyse();
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }

        #endregion Movie Tests

        #region NFL

        [TestMethod]
        public void TestMediaInfoNflTeam()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Cincinnatti Bengals - Denver Broncos 2014-12-24.mp4", SearchOption.AllDirectories).ToList();

            foreach (var mi in testCandidates.Select(item => new MediaInfo(item)))
            {
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
                Assert.IsFalse(mi.IsSoccer);
                Assert.IsTrue(mi.IsNfl);
            }
        }

        [TestMethod]
        public void TestMediaInfoNFLAnalysis()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "NFL WK 15 12-14-2014 49ers at Seahawks (Condensed) (1280x720) [Phr0stY].mkv", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsTrue(mi.IsNfl);
                Assert.IsFalse(mi.IsBook);
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }

        #endregion NFL

        #region Magazines Testing

        [TestMethod]
        public void TestIsNotaMag()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Apress - Beginning ASP.NET 4.pdf", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.MagazineFolder = ".\\Output\\Magazines\\";
                mi.Analyse();
                Assert.IsFalse(mi.IsMagazine);
                Assert.IsTrue(mi.IsBook);
            }
        }

        [TestMethod]
        public void TestPcFormatIsMag()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "PC Format 2014-12.pdf", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.MagazineFolder = ".\\Output\\Magazines\\";
                mi.Analyse();
                Assert.IsTrue(mi.IsMagazine);
            }
        }

        [TestMethod]
        public void TestMacWorldtIsMag()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Macworld USA - February 2015.pdf", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.MagazineFolder = ".\\Output\\Magazines\\";
                mi.Analyse();
                Assert.IsTrue(mi.IsMagazine);
            }
        }

        [TestMethod]
        public void TestMediaInfoMagList()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "PC Format 2014-12.pdf", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.MagazineFolder = ".\\Output\\Magazines\\";
                mi.GetMagList();
                mi.Analyse();
                Assert.IsTrue(mi.Magazines.Count > 0);
                Assert.IsTrue(mi.IsMagazine);
            }
        }

        [TestMethod]
        public void TestMediaInfoBogusMagFolder()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "PC Format 2014-12.pdf", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item) { MagazineFolder = ".//Output//Bogus//" };
                mi.GetMagList();
                Assert.IsNull(mi.Magazines);
            }
        }

        [TestMethod]
        public void TestMediaInfoPdf()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Computeractive UK Issue 438 - December 10, 2014.pdf", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.MagazineFolder = ".//Output//Magazines//";
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsTrue(mi.Title.Equals("Computeractive UK Issue"));
                Assert.IsFalse(mi.IsTV);
                Assert.IsTrue(mi.IsMagazine);
            }
        }

        [TestMethod]
        public void TestAllNumbers()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Computeractive UK Issue 438 - December 10, 2014.pdf", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                Assert.IsTrue(mi.ContainsAllNumbers("438"));
                Assert.IsTrue(mi.ContainsAllNumbers("408"));
                Assert.IsFalse(mi.ContainsAllNumbers("mp4"));
                Assert.IsFalse(mi.ContainsAllNumbers("m4o"));
                Assert.IsFalse(mi.ContainsAllNumbers("4pm"));
                Assert.IsFalse(mi.ContainsAllNumbers("4p4"));
            }
        }

        [TestMethod]
        public void TestMediaInfoMagazineAnalysis()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "PC Format 2014-12.pdf", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item) { MagazineFolder = ".//Output//Magazines//" };
                mi.Analyse();
                Assert.IsFalse(mi.IsNfl);
                Assert.IsFalse(mi.IsBook);
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
                Assert.IsTrue(mi.IsMagazine);
            }
        }

        #endregion Magazines Testing

        #region TV show tests

        [TestMethod]
        public void TestTvHousewives()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "The_Real_Housewives_of_Atlanta_S07E19[480i_HDTV_H.264_mp4].mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsTrue(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
                Assert.IsTrue(mi.Season.Equals(7));
                Assert.IsTrue(mi.Episode.Equals(19));
            }
        }

        [TestMethod]
        public void TestTvCriminalMinds()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "criminal minds 1017 hdtv lol.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsTrue(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
                Assert.IsTrue(mi.Season.Equals(10));
                Assert.IsTrue(mi.Episode.Equals(17));
            }
        }

        [TestMethod]
        public void TestTvHauntingAustralia()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Haunting-Australia.S01E01.Woodford.Academy.HDTV.x264-SPASM.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsTrue(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
                Assert.IsTrue(mi.Episode.Equals(1));
            }
        }

        [TestMethod]
        public void TestTvTest()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "testvideo1.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }

        [TestMethod]
        public void TestTvFlash()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "the.flash.2014.116.hdtv-lol.mp4 ", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsTrue(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }

        [TestMethod]
        public void TestTvCastle()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "castle.2009.717.hdtv-lol.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsTrue(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }


        [TestMethod]
        public void TestTvWord()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "ncis.1217.hdtv-lol.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                var isTV = mi.IsTvWord("1217");
                //  in progress files are ignored
                Assert.IsTrue(isTV);
            }
        }

        [TestMethod]
        public void TestMediaInfoNCIS()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "ncis.1217.hdtv-lol.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  in progress files are ignored
                Assert.IsTrue(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }

        [TestMethod]
        public void TestMediaInfoInProgress()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Judge Judy S19E01.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  in progress files are ignored
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }

        [TestMethod]
        public void TestMediaInfoJudgeJudy()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "judge.judy.2015.04.28b.mackey.vs.peak.hdtv.x264-daview.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  in progress files are ignored
                Assert.IsTrue(mi.Title.Equals("Judge Judy"));
                Assert.IsTrue(mi.Season.Equals(20));
                Assert.IsTrue(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
            }
        }

        [TestMethod]
        public void TestMediaInfoJudgeJudySeason20()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Judge Judy 2016 07 06 S20E220.mkv", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  in progress files are ignored
                Assert.IsTrue(mi.Title.Equals("Judge Judy"), "Title is not Judge Judy");
                Assert.IsTrue(mi.Season.Equals(20), "Season is not 20");
                Assert.IsTrue(mi.IsTV, "Not TV");
                Assert.IsFalse(mi.IsMovie, "Is movie");
            }
        }

        [TestMethod]
        public void TestMediaInfoShieldTitle()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Marvels.Agents.of.S.H.I.E.L.D.S02E10.HDTV.x264-KILLERS.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                var title = mi.DetermineTitle();
                //  full stops are stripped out of titles
                Assert.IsTrue(title.Equals("Marvels Agents of S H I E L D"));
            }
        }

        [TestMethod]
        public void TestMediaInfoGoodnightSweetheart()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Goodnight Sweetheart  - S01 - E01 - Rites of Passage.avi", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsTrue(mi.Season.Equals(0));  //  basically we dont want an exception
            }
        }

        [TestMethod]
        public void TestMediaInfoSliders()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Sliders.1x01_1x02.Pilot.AC3.DVDRip_XviD-FoV.avi",
               SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                Assert.IsTrue(mi.Episode.Equals(1));
            }
        }

        [TestMethod]
        public void TestMediaInfoTitleWithYear()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Castle.2009.S07E10.HDTV.x264-LOL.[VTV].mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                var title = mi.DetermineTitle();
                //  Numbers are stripped out of titles
                Assert.IsTrue(title.Equals("Castle [2009]"));
            }
        }

        [TestMethod]
        public void TestMediaInfoAlternatTvWord()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "arrow.309.hdtv-lol.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsTrue(mi.Title.Equals("arrow"));
                Assert.IsTrue(mi.IsTV);
                Assert.IsTrue(mi.Season == 3);
                Assert.IsTrue(mi.Episode == 9);
            }
        }

        [TestMethod]
        public void TestMediaInfoTvWord()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "married_with_children.1x01.pilot.dvdrip_xvid-fov.avi", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                var tvWord = mi.IsTvWord("1x01");
                Assert.IsTrue(tvWord);
            }
        }

        [TestMethod]
        public void TestMediaInfoNotTvWord()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "married_with_children.1x01.pilot.dvdrip_xvid-fov.avi", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                var tvWord = mi.IsTvWord("XVID");
                Assert.IsFalse(tvWord);
            }
        }

        [TestMethod]
        public void TestMediaInfoTvWordType3()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "married_with_children.1x02.pilot.dvdrip_xvid-fov.avi", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsTrue(mi.Title.Equals("married with children"));
                Assert.IsTrue(mi.IsTV);
                Assert.IsTrue(mi.Season == 1);
                Assert.IsTrue(mi.Episode == 2);
            }
        }

        [TestMethod]
        public void TestMediaInfo720p()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "canadas.worst.driver.s10e08.720p.HDTV.x264.mp4", SearchOption.AllDirectories).ToList();

            foreach (var item in testCandidates)
            {
                var mi = new MediaInfo(item);
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsTrue(mi.Title.Equals("canadas worst driver"));
                Assert.IsTrue(mi.IsTV);
                Assert.IsTrue(mi.Season == 10);
                Assert.IsTrue(mi.Episode == 8);
            }
        }

        #endregion TV show tests

        #region Software Tests

        [TestMethod]
        public void TestMediaInfoExe()
        {
            var testCandidates = Directory.GetFiles(".//Output//DL//", "Setup Novicorp WinToFlash 0.8.0000 beta[PreCracked].exe", SearchOption.AllDirectories).ToList();

            foreach (var mi in testCandidates.Select(item => new MediaInfo(item)))
            {
                Assert.IsFalse(mi.HasValidExt());
            }
        }

        #endregion Software Tests

        #region Soccer Tests

        [TestMethod]
        public void TestMediaInfoSoccerFACup()
        {
            var testCandidates = Directory.GetFiles(
                path: ".//Output//DL//",
                searchPattern: "FA Cup.2015.03.09.1-4finals.Manchester United v. Arsenal.720p.mkv",
                searchOption: SearchOption.AllDirectories).ToList();

            foreach (var mi in testCandidates.Select(item => new MediaInfo(item)))
            {
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
                Assert.IsTrue(mi.IsSoccer);
            }
        }

        [TestMethod]
        public void TestMediaInfoSoccer()
        {
            var testCandidates = Directory.GetFiles(
                path: ".//Output//DL//",
                searchPattern: "EPL - Match of Day 720p 20.12.2014.mkv",
                searchOption: SearchOption.AllDirectories).ToList();

            foreach (var mi in testCandidates.Select(item => new MediaInfo(item)))
            {
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
                Assert.IsTrue(mi.IsSoccer);
            }
        }

        [TestMethod]
        public void TestMatchOfTheDayIsSoccer()
        {
            var testCandidates = Directory.GetFiles(
                path: ".//Output//DL//",
                searchPattern: "Match.Of.The.Day.2020.02.08.720p.HDTV.x264-ACES[eztv].mkv",
                searchOption: SearchOption.AllDirectories).ToList();

            foreach (var mi in testCandidates.Select(item => new MediaInfo(item)))
            {
                mi.Analyse();
                //  Numbers are stripped out of titles
                Assert.IsFalse(mi.IsTV);
                Assert.IsFalse(mi.IsMovie);
                Assert.IsTrue(mi.IsSoccer);
            }
        }

        #endregion Soccer Tests

        #region Miscelaneous Tests

        [TestMethod]
        public void TestMediaInfoFileNameTooLong()
        {
            var testCandidates = new string[1];
            testCandidates[0] = @"d:\\shares\\Downloads\InProgress\MSTYCSASA - Huge ebook collection on many topics\Stop Working ... Start Living How I Retired at 36 ... without Winning the Lottery-Mantesh\Stop Working ... Start Living How I Retired at 36 ... without Winning the Lottery-Mantesh.pdf.!ut";

            foreach (var mi in testCandidates.Select(item => new MediaInfo(item)))
            {
                Assert.IsFalse(mi.IsValid);
            }
        }

        #endregion Miscelaneous Tests
    }
}