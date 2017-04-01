using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System.IO;

namespace Gerard.Tests
{
	[TestClass]
	public class TippingControllerTests
	{
		[TestMethod]
		public void TestPredictions()
		{
			var sut = new TippingController();
			sut.Index(season:"2015");
			var fileOut = sut.OutputFilename;
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}
	}
}
