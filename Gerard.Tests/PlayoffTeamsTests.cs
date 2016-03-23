using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
   [TestClass]
   public class PlayoffTeamsTests
   {
      [TestMethod]
      public void TestDoPlayoffTeamsJob()  //   
      {
         var sut = new PlayOffTeamsJob(new TimeKeeper());
         var outcome = sut.DoJob();
         Assert.IsFalse(string.IsNullOrEmpty(outcome));
      }

		[TestMethod]
		public void TestTimeToDoPlayoffTeamsJob()  //   
		{
			var sut = new PlayOffTeamsJob(new FakeTimeKeeper(isPreSeason: true, isPeakTime: false));
			string whyNot;
			Assert.IsFalse(sut.IsTimeTodo(out whyNot));
			Console.WriteLine(whyNot);
		}
   }
}
