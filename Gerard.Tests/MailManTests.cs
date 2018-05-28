using Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
	[TestClass]
	public class MailManTests
	{
		private MailMan2 sut;

		[TestInitialize]
		public void TestInitialize()
		{
			sut = SystemUnderTest();
		}

		private static MailMan2 SystemUnderTest()
		{
			return new MailMan2( new ConfigReader() );
		}

		[TestMethod]
		public void TestSendMail()
		{
			sut.AddRecipients( "quarterback16@iinet.com.au" );
			sut.SendMail( message: "Test", subject: "Test" );
		}

		[TestMethod]
		public void TestSendMailWithAttachment()
		{
			sut.AddRecipients( "quarterback16@iinet.com.au" );
			sut.SendMail(
                message: "Test",
                subject: "Test",
                attachment: "l:\\RssScanner-2018-05-12.log" );
		}

		[TestMethod]
		public void TestPokeServer()
		{
			MailMan.PokeServer( "192.168.1.113", 25 );
		}
	}
}