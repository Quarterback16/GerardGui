using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
	[TestClass]
	public class IsRetiredTest
	{
		[TestMethod]
		public void TestIsRetired()
		{
			var isRetired = Convert.ToBoolean("True");

			Assert.IsTrue(isRetired);
		}
	}
}
