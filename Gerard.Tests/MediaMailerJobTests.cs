using Butler.Models;
using Helpers;
using Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class MediaMailerJobTests
	{
		private MediaMailerJob sut;
		protected IMailMan mailMan;

		[TestInitialize]
		public void TestInitialize()
		{
			sut = SystemUnderTest();
		}

		private MediaMailerJob SystemUnderTest()
		{
			var configReader = new ConfigReader();
			mailMan = new MailMan2( configReader );
			IDetectLogFiles logFileDetector = new MediaLogDetector();
			var fakeTimeKeeper = new FakeTimeKeeper(new DateTime( 2017, 8, 4 ) );
			return new MediaMailerJob( mailMan, logFileDetector, configReader, fakeTimeKeeper );
		}

		[TestMethod]
		public void TestTimetoDoMediaMailer()
		{
			Assert.IsTrue( sut.IsTimeTodo( out string whyNot ) );
			Console.WriteLine( whyNot );
		}

		[TestMethod]
		public void TestMediaMailerJob()
		{
			sut.DoJob();
			Assert.IsTrue( sut.LogsMailed == 0 );  //  already mailed
		}

		[TestMethod]
		public void TestMediaMailerNumberOfRecipients()
		{
			var result = mailMan.RecipientCount();
			Assert.AreEqual( actual: result, expected: 2 );
		}
	}
}