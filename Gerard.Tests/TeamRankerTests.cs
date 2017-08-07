using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
	[TestClass]
	public class TeamRankerTests
	{
		[TestMethod]
		public void TestTeamRankerJob()
		{
			var sut = new RankingsJob( new FakeTimeKeeper( "2017", "01" ) );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

      [TestMethod]
      public void ShouldInitialiseTheSeasonFromLastSeason()
      {
         var sut = new RankingsJob( 
            new FakeTimeKeeper( new System.DateTime( 2016,9,9 ) ),
            force: true );
         var outcome = sut.DoJob();
         Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
      }
   }
}
