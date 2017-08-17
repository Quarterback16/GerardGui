using Microsoft.VisualStudio.TestTools.UnitTesting;
using Butler;
using Butler.Models;
using System.Collections.Generic;
using Gerard.Tests.Fakes;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class JobLogTests
	{
		#region  cut Initialisation

		private JobLog cut;

		[TestInitialize]
		public void TestInitialize()
		{
			cut = ClassUnderTest();
		}

		private static JobLog ClassUnderTest()
		{
			var testJobList = new List<Job>();
			var testJob = new FakeJob( "Fake Job 1", new TimeSpan(1,0,0) );
			testJobList.Add( testJob );
			var testJob2 = new FakeJob( "Fake Job 2", new TimeSpan( 0, 34, 0 ) );
			testJobList.Add( testJob2 );
			var testJob3 = new FakeJob( "Fake Job 3", new TimeSpan( 0, 0, 0 ), onHold: true );
			testJobList.Add( testJob3);
			return new JobLog( testJobList );
		}

		#endregion

		[TestMethod]
		public void Should_Output_Some_Lines()
		{
			var result = cut.Generate();
			Assert.IsTrue( result.Count > 0 );
			DumpLines( result );
		}

		private static void DumpLines( List<string> result )
		{
			foreach ( var line in result )
			{
				Console.WriteLine( line );
			}
		}

		[TestMethod]
		public void Longest_JobShouldBe_at_the_top()
		{
			var result = cut.Generate();
			var longest = result[ 0 ].ToString();
			Assert.AreEqual( expected: "Job: Fake Job 1 ran for 01:00:00", actual: longest );
		}

		[TestMethod]
		public void Should_Not_Display_On_Hold_Jobs()
		{
			var result = cut.Generate();
			DumpLines( result );
			Assert.IsTrue( result.Count == 2 );
		}
	}
}
