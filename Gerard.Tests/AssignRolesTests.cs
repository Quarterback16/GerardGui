using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using Butler.Models;


namespace Gerard.Tests
{
   [TestClass]
   public class AssignRolesTests
   {
      [TestMethod]
      public void TestDoAssignRolesJob()  //  8 mins
      {
         var sut = new AssignRolesJob(new TimeKeeper());
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
      }

      [TestMethod]
      public void TestTimetoDoJob()
      {
			var sut = new AssignRolesJob( new FakeTimeKeeper( isPreSeason: true, isPeakTime: false ) );
         string whyNot;
         Assert.IsFalse(sut.IsTimeTodo(out whyNot));
         Console.WriteLine(whyNot);
      }



      [TestMethod]
      public void TestNextGame()
      {
         var sut = new NflTeam( "SS" );
         var nextGame = sut.NextGameOrBye();
         Assert.IsNotNull( nextGame );
      }

      [TestMethod]
      public void TestSingleTeam()
      {
         var sut = new RoleAssignmentReport("BB");
         sut.RenderAsHtml();
         Assert.IsTrue( sut.RunTime > new TimeSpan( 0,0,0 ) );
      }

      [TestMethod]
      public void TestAceBack()
      {
         var sut = new NflTeam(  "BB" );
         sut.LoadRushUnit();
         Assert.IsTrue( string.IsNullOrEmpty( sut.RushUnit.AceBack.PlayerCode ) );
      }

      [TestMethod]
      public void TestWeekSelection()
      {
         var week = Utility.PreviousWeekAsString();
         Assert.IsFalse(  string.IsNullOrEmpty( week )  );
      }

      [TestMethod]
      public void TestAllLoadAnalysis()
      {
         const string week = "07"; //  the week we are looking at the stats of
         var season = Utility.CurrentSeason();
         var s = new NflSeason(season, true);
         foreach (var t in s.TeamList)
         {
            t.LoadRushUnit();
            t.RushUnit.LoadCarries(season, week);
            t.LoadPassUnit();
            t.PassUnit.AnalyseQuarterbacks(season, week);
            t.PassUnit.AnalyseWideouts(season, week);
            t.PassUnit.AnalyseTightends(season, week);
         }
      }

      [TestMethod]
      public void TestAnalisePassingYardage()
      {
         var sut = new NflTeam("SL");
         sut.LoadPassUnit();
         Assert.IsTrue(sut.PassUnit.Quarterbacks.Count > 0);
         Utility.Announce(string.Format("Loaded {0} QBs", sut.PassUnit.Quarterbacks.Count));

         sut.PassUnit.AnalyseQuarterbacks("2014", "04");
      }

      [TestMethod]
      public void TestAnaliseReceptionYardage()
      {
         var sut = new NflTeam("JJ");
         sut.LoadPassUnit();
         Assert.IsTrue(sut.PassUnit.Receivers.Count > 0);
         Utility.Announce(string.Format("Loaded {0} receivers", sut.PassUnit.Receivers.Count));

         sut.PassUnit.DumpUnit();
         sut.PassUnit.AnalyseWideouts("2014", "07");
//         sut.PassUnit.AnalyseTightends("2014", "04");
      }

      [TestMethod]
      public void TestLoadUnitCarries()
      {
         var sut = new NflTeam("SD");
         sut.LoadRushUnit();
         Assert.IsTrue(sut.RushUnit.Runners.Count > 0);			
         Assert.IsTrue(sut.RushUnit.Runners.Count < 10, string.Format("{0} runners", sut.RushUnit.Runners.Count));
         Utility.Announce(string.Format("Loaded {0} runners", sut.RushUnit.Runners.Count));
         //Assert.IsFalse(sut.RushUnit.HasIntegrityError());
         //sut.RushUnit.DumpUnit();
         sut.RushUnit.LoadCarries("2013", "12");
      }

      [TestMethod]
      public void TestGettingPenaltyScores()
      {
         var ds = Utility.TflWs.PenaltyScores( "2013", "12", "AF" );
         Assert.IsNotNull(ds);
      }

      [TestMethod]
      public void TestGetShortYardageBack()
      {
         var sut = new NflTeam("AF");
         sut.LoadRushUnit();
         var sh = sut.RushUnit.GetShortYardageBack("2013", "12", "AF");
         Assert.AreEqual("JACKST02",sh);
      }
      
      [TestMethod]
      public void TestGettingStatsForCarries()
      {
         var carries = Utility.TflWs.PlayerStats(Constants.K_STATCODE_RUSHING_CARRIES, "2013", "12", "MOREKN01" );
         Assert.IsTrue( carries.Equals("37" ) );
      }

      [TestMethod]
      public void TestStoringRole()
      {
         Utility.TflWs.StorePlayerRoleAndPos( "S","QB", "MONTJO01");
         var ds = Utility.TflWs.GetPlayer("MONTJO01");
         var dt = ds.Tables[0];
         var dr = dt.Rows[0];
         Assert.IsTrue(dr["POSDESC"].ToString().Trim().Equals("QB"));
         Assert.IsTrue(dr["ROLE"].ToString().Equals("S"));
      }

   }
}
