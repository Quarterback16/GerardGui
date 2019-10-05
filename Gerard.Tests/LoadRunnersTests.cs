using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class LoadRunnersTests
	{
		#region  cut Initialisation

		private LoadRunners cut;

		[TestInitialize]
		public void TestInitialize()
		{
			cut = ClassUnderTest();
		}

		private static LoadRunners ClassUnderTest()
		{
			return new LoadRunners();
		}

		#endregion

		[TestMethod]
		public void TestLoaderFindsRunners()
		{
			var result = cut.Load( "PS" );
			Assert.IsTrue( result .Count > 0);
		}
	}
}
