using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler.Models;

namespace Gerard.Tests
{
	[TestClass]
	public class FaMarketTests
	{
		[TestMethod]
		public void TestFaMarketJob()
		{
			var sut = new FreeAgentMarketJob( new FakeTimeKeeper() );
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
