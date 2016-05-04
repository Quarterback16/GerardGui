using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler.Models;

namespace Gerard.Tests
{
	[TestClass]
	public class FaMarketTests
	{
		[TestMethod]
		public void TestFaMarketJob()  //2016-05-02 : 15 mins
		{
			var sut = new FreeAgentMarketJob( new FakeTimeKeeper(season:"2016") );
			var outcome = sut.DoJob();
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}

		[TestMethod]
		public void TestTimetoDoJob()
		{
			var sut = new FreeAgentMarketJob( new FakeTimeKeeper( isPreSeason:true, isPeakTime:false) );
			string whyNot;
			Assert.IsTrue( sut.IsTimeTodo( out whyNot ) );
			Console.WriteLine( whyNot );
		}
	}
}
