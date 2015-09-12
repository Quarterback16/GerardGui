using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using TFLLib;
using System.IO;
using Butler.Models;

namespace Gerard.Tests
{
   [TestClass]
   public class MatchupReportTests
   {
      [TestMethod]
      public void TestMatchUpReportSeasonOpener2014()
      {
         var g = new NFLGame("2014:01-A");
         var mur = new MatchupReport(g);
         mur.Render(writeProjection: false);
         Assert.IsTrue(File.Exists(mur.FileOut), string.Format("Cannot find {0}", mur.FileOut));
      }
   }
}
