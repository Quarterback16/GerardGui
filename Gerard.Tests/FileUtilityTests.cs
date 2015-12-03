using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;

namespace Gerard.Tests
{
	[TestClass]
	public class FileUtilityTests
	{
		[TestMethod]
		public void TestFileCount()
		{
			var testCount = FileUtility.CountFilesInDirectory(".\\TestFolder\\");
			Assert.AreEqual(expected: 1, actual: testCount);
		}

		[TestMethod]
		public void TestFileCountWithDirs()
		{
			var testCount = FileUtility.CountFilesInDirectory(".\\TestVesuviusTfl\\");
			Assert.AreEqual(expected: 4, actual: testCount);
		}
	}
}
