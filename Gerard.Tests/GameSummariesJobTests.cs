using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;


namespace Gerard.Tests
{
	[TestClass]
	public class GameSummariesJobTests
	{
		[TestMethod]
		public void TestDoGameSummariesJob()
		{
			var sut = new GameSummariesJob( new FakeTimeKeeper( "2016", "18" ) );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestTimetoDoGameSummaries()
		{
			var sut = new GameSummariesJob( new TimeKeeper( clock: null ) );
			string whyNot;
			Assert.IsFalse( sut.IsTimeTodo( out whyNot ) );
			Console.WriteLine( whyNot );
		}
	}
}
