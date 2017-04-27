using System;
using Butler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
	[TestClass]
	public class DropboxCopyToReginaJobTests
	{
		[TestMethod]
		public void TestDropboxCopyToReginaJob()
		{
			var sut = new DropboxCopyToReginaJob(
				new FakeTimeKeeper( season: "2017" ),
				sourceDir: "y:\\{0}\\",
				destDir: "w:\\web\\medialists\\dropbox\\gridstat\\{0}"
				);

			var outcome = sut.DoJob();
			Console.WriteLine( "outcome={0}", outcome );
			Assert.IsFalse( string.IsNullOrEmpty( outcome ) );
		}
	}
}
