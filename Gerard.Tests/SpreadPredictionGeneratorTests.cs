using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.PredictionGenerators;

namespace Gerard.Tests
{
	[TestClass]
	public class SpreadPredictionGeneratorTests
	{
		[TestMethod]
		public void TestSpreadGenerator()  // 15 seconds
		{
			var storer = new DbfPredictionStorer();
			var sut = new SpreadPredictionGenerator("2015", storer);
			sut.GeneratePredictions();
		}
	}
}
