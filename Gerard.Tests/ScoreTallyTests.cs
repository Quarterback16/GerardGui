using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System.IO;

namespace Gerard.Tests
{
	[TestClass]
	public class ScoreTallyTests
	{
		[TestMethod]
		public void TestPredictedOutput()
		{
			var sut = new ScoreTally( "2015", "All Teams", true );
			sut.Render();
			var fileOut = sut.FileName();
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}

		[TestMethod]
		public void TestActualOutput()
		{
			var sut = new ScoreTally( "2014", "Actuals", usingPredictions: false )
			{
				ForceRefresh = false
			};
			sut.Render();
			var fileOut = sut.FileName();
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}
	}
}
