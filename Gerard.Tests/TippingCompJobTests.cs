using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Gerard.Tests
{
	[TestClass]
	public class TippingCompJobTests
	{
		[TestMethod]
		public void TestPredictions()
		{
			var job = new TippingCompJob(new FakeTimeKeeper(season: "2016", week: "01"));
			var fileOut = job.DoJob();
			Console.WriteLine( $"Output sent to {fileOut}" );
			Assert.IsTrue( File.Exists( fileOut ), string.Format( "Cannot find {0}", fileOut ) );
		}
	}
}
