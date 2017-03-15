using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System.IO;

namespace Gerard.Tests
{
   [TestClass]
   public class GameSummaryTests
   {

      [TestMethod]
      public void TestGameSummary()
      {
         var game = new NFLGame("2016:02-J");

         var sut = new GameSummary(game);
         sut.Render();
         var fileOut = sut.FileName();

         Assert.IsTrue(File.Exists(fileOut), string.Format("Cannot find {0}", fileOut));
      }

		[TestMethod]
		public void TestGameSummariesForAWeek()
		{
			var week = new NFLWeek( seasonIn:"2016", weekIn: 1 );

			var sut = new GameSummary( week.GamesList() );
			sut.Render();
			var fileOut = sut.FileName();

			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}


		[TestMethod]
      public void TestSpecialTeamScores()
      {
         var game = new NFLGame("2014:13-N");

         Assert.IsTrue( game.HomeTDs.Equals(0) );
      }

   }
}
