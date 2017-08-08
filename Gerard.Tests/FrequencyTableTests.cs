using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class FrequencyTableTests
	{
		#region  Sut Initialisation

		private FrequencyTable sut;

		[TestInitialize]
		public void TestInitialize()
		{
			sut = SystemUnderTest();
		}

		private static FrequencyTable SystemUnderTest()
		{
			return new FrequencyTable("TestFrequencyTable");
		}

		#endregion
	}
}
