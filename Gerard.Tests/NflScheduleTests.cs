using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class NflScheduleTests
   {
      private NFLSchedule _cut;

      [TestInitialize]
      public void TestInitialize()
      {
         var theTeam = new NflTeam( "PS" );
         _cut = new NFLSchedule( season:"2016", team: theTeam );
      }

      [TestCleanup]
      public void TearDown()
      {
      }

      [TestMethod]
      public void The_Team_Made_The_Playoffs()
      {
         var result = _cut.GameList;
         Assert.IsTrue( result.Count > 16 );  // made the playoffs
      }
   }
}
