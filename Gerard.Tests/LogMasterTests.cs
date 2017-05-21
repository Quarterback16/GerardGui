using Helpers;
using Helpers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gerard.Tests
{
	[TestClass]
	public class LogMasterTests
	{
		private LogMaster sut;

		[TestInitialize]
		public void TestInitialize()
		{
			sut = SystemUnderTest();
		}

		private static LogMaster SystemUnderTest()
		{
			return new LogMaster( ".\\xml\\mail-list.xml" );
		}

		[TestMethod]
		public void TestLoad()
		{
			Assert.IsTrue( sut.TheHt.Count == 3 );
		}

		[TestMethod]
		public void TestNode()
		{
			var myEnumerator = sut.TheHt.GetEnumerator();

			while ( myEnumerator.MoveNext() )
			{
				var testNode1 = ( LogItem ) myEnumerator.Value;
				WriteLogNode( testNode1 );
				Assert.IsTrue( testNode1.Subject == "Article Scanner log test" );
				break;
			}

		}

		private void WriteLogNode( LogItem m )
		{
			System.Console.WriteLine( $"Subject    : {m.Subject}");
			System.Console.WriteLine( $"Recipients : {m.Recipients}" );
		}
	}
}