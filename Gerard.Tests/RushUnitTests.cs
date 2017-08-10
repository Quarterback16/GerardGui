using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class RushUnitTests
	{
		[TestMethod]
		public void TestFakeData_BB_HasAnAceBack()
		{
			var team = new FakeNflTeam()
			{
				RunUnit = new FakeRushUnit()
			};
			var results = team.RunUnit.Load("BB");
			Assert.IsTrue( team.RunUnit.IsAceBack );
		}

		[TestMethod]
		public void TestFakeData_NE_HasTwoRunners()
		{
			var team = new FakeNflTeam()
			{
				RunUnit = new FakeRushUnit()
			};
			var results = team.RunUnit.Load( "NE" );
			Assert.IsTrue( team.RunUnit.Runners.Count == 2 );
		}

		[TestMethod]
		public void TestFakeData_NE_HasNoIntegrityErrors()
		{
			var team = new FakeNflTeam()
			{
				RunUnit = new FakeRushUnit()
			};
			var results = team.RunUnit.Load( "NE" );
			Assert.IsFalse( team.RunUnit.HasIntegrityError() );
		}

		[TestMethod]
		public void TestFakeData_AF_HasNoIntegrityErrors()
		{
			var team = new FakeNflTeam()
			{
				RunUnit = new FakeRushUnit()
			};
			var results = team.RunUnit.Load( "AF" );
			Assert.IsFalse( team.RunUnit.HasIntegrityError() );
		}

		[TestMethod]
		public void TestFakeData_BB_HasNoIntegrityErrors()
		{
			var team = new FakeNflTeam()
			{
				RunUnit = new FakeRushUnit()
			};
			var results = team.RunUnit.Load( "BB" );
			Assert.IsFalse( team.RunUnit.HasIntegrityError() );
		}

		[TestMethod]
		public void TestFakeData_BR_HasNoIntegrityErrors()
		{
			var team = new FakeNflTeam()
			{
				RunUnit = new FakeRushUnit()
			};
			var results = team.RunUnit.Load( "BR" );
			Assert.IsFalse( team.RunUnit.HasIntegrityError() );
		}


		[TestMethod]
		public void TestLoad()
		{
			var team = new NflTeam( "PS" );
			var ru = team.LoadRushUnit();
			Console.WriteLine( "   >>> Rush unit loaded {0} rushers; Ace back {1}",
			   team.RunUnit.Runners.Count, team.RunUnit.AceBack );
			Assert.IsTrue( team.RunUnit.Runners.Count < 50 && team.RunUnit.Runners.Count > 0 );
			Assert.IsFalse( team.RunUnit.HasIntegrityError() );
		}

		[TestMethod]
		public void TestDoubleLoad()
		{
			var team = new NflTeam( "PS" );
			var ru = team.LoadRushUnit();
			Console.WriteLine( "   >>> Rush unit loaded {0} rushers; Ace back {1}",
			   team.RunUnit.Runners.Count, team.RunUnit.AceBack );
			var count1 = ru.Count;
			var ru2 = team.LoadRushUnit();
			var count2 = ru2.Count;
			Assert.IsTrue( count1 == count2 );
			Assert.IsFalse( team.RunUnit.HasIntegrityError() );
		}
	}
}