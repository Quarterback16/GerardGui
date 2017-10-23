using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using RosterLib.RosterGridReports;
using System;
using System.IO;

namespace Gerard.Tests
{
	[TestClass]
	public class PickupSummaryTests
	{
		[TestMethod]
		public void Constructor_IntantiatesAnObject()
		{
			var sut = new PickupSummary( 
				new FakeTimeKeeper( season: "2017", week: "08" ), 8 );
			Assert.IsNotNull( sut );
		}

		[TestMethod]
		public void Summary_GeneratesOutput()
		{
			var sut = new PickupSummary(
				new FakeTimeKeeper( season: "2017", week: "08" ), 8 );
			sut.RenderAsHtml();
			Console.WriteLine( $"{sut.Name} rendered to {sut.FileOut}");
			Assert.IsTrue( File.Exists( sut.FileOut ) );
		}

		[TestMethod]
		public void Summary_AcceptsPickups()
		{
			var sut = new PickupSummary(
				new FakeTimeKeeper( season: "2017", week: "08" ), 8 );
			sut.AddPickup( new Pickup
			{
				Name = "KForbath",
				CategoryCode = Constants.K_KICKER_CAT,
				Opp = "@MV -10",
				ProjPts = 22
			} );
			sut.AddPickup( new Pickup
			{
				Name = "ABrown",
				CategoryCode = Constants.K_RECEIVER_CAT,
				Opp = "@CI -6",
				ProjPts = 22
			} );
			sut.AddPickup( new Pickup
			{
				Name = "SColonna",
				CategoryCode = Constants.K_QUARTERBACK_CAT,
				Opp = "@SF +6",
				ProjPts = 100
			} );
			sut.AddPickup( new Pickup
			{
				Name = "APeterson",
				CategoryCode = Constants.K_RUNNINGBACK_CAT,
				Opp = "@HT +9",
				ProjPts = 6
			} );
			sut.RenderAsHtml();
			Console.WriteLine( $"{sut.Name} rendered to {sut.FileOut}" );
			Assert.IsTrue( File.Exists( sut.FileOut ) );
		}
	}
}
