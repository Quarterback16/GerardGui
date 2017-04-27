using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
	[TestClass]
	public class DropBoxCopyTflToVesuviusJobTests
	{
		[TestMethod]
		public void TestDropBoxCopyTflToVesuviusJob()
		{
			var sut = new DropBoxCopyTflToVesuviusJob( new FakeTimeKeeper(isPreSeason:false, isPeakTime:false))
			{
				SourceDir = "g:\\FileSync\\SyncProjects\\GerardGui\\Gerard.Tests\\bin\\Debug\\TestDropboxTfl",
				DestDir = "g:\\FileSync\\SyncProjects\\GerardGui\\Gerard.Tests\\bin\\Debug\\TestVesuviusTfl"
			};
			var outcome = sut.DoJob();
			Console.WriteLine("outcome={0}", outcome);
			Assert.IsFalse(string.IsNullOrEmpty(outcome));
		}


		[TestMethod]
		public void TestTimetoDoDropBoxCopyTflToVesuviusJob()
		{
			var sut = new DropBoxCopyTflToVesuviusJob(new FakeTimeKeeper(isPreSeason: false, isPeakTime: true))
			{
				SourceDir = "g:\\FileSync\\SyncProjects\\GerardGui\\Gerard.Tests\\bin\\Debug\\TestDropboxTfl",
				DestDir = "g:\\FileSync\\SyncProjects\\GerardGui\\Gerard.Tests\\bin\\Debug\\TestVesuviusTfl"
			};
			Assert.IsFalse( sut.IsTimeTodo( out string whyNot ) );
			Console.WriteLine(whyNot);
		}

	}
}
