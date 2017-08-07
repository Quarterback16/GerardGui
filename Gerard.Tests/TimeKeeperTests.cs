using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosterLib;
using System;

namespace Gerard.Tests
{
	[TestClass]
	public class TimeKeeperTests
	{
		[TestMethod]
		public void TestWhatWeekItIs()
		{
			var sut = new TimeKeeper( clock: null );
			Console.WriteLine( $"Season : {sut.Season} Week {sut.Week}" );
			Console.WriteLine( $"Schedule Available :{sut.ScheduleAvailable}" );
			Console.WriteLine( $"IsItPreseason      :{sut.IsItPreseason()}" );
			Console.WriteLine( $"IsItQuietTime      :{sut.IsItQuietTime()}" );
			Console.WriteLine( $"IsItPeakTime       :{sut.IsItPeakTime()}" );
			Console.WriteLine( $"IsItWednesday      :{sut.IsItWednesday(DateTime.Now)}" );
			Console.WriteLine( $"GetDate            :{sut.GetDate()}" );
			Console.WriteLine( $"CurrentSeason      :{sut.CurrentSeason()}" );
			Console.WriteLine( $"CurrentWeek        :{sut.CurrentWeek()}" );
			Console.WriteLine( $"PreviousWeek       :{sut.PreviousWeek()}" );
			Console.WriteLine( $"CurrentDateTime    :{sut.CurrentDateTime()}" );
			Console.WriteLine( $"PreviousSeason     :{sut.PreviousSeason()}" );
			Assert.IsNotNull( sut );
		}

		[TestMethod]
		public void TestCurrentSeason()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2017, 01, 05 ) ) );  // set clock to March
			Assert.AreEqual( expected: "2016", actual: sut.CurrentSeason() );
		}

		[TestMethod]
		public void TestWeekCutsOverOnMonday()
		{
			int lastWeek = 0;
			for ( int d = 0; d < 7; d++ )
			{
				var day = d + 1;
				var testDate = new DateTime( 2016, 12, day );
				var sut = new TimeKeeper( new FakeClock( testDate ) );
				Console.WriteLine( "{0,10:dddd} {0,10:d} {1} {2}", testDate, sut.Season, sut.Week );
				if ( testDate.ToString( "dddd" ).Equals( "Monday" ) )
					Assert.IsTrue( Int32.Parse( sut.Week ) > lastWeek );
				lastWeek = Int32.Parse( sut.Week );
			}
		}

		[TestMethod]
		public void TestCurrentWeek()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2015, 03, 16 ) ) );  // set clock to March
			Assert.AreEqual( sut.CurrentWeek(), 0 );
		}

		[TestMethod]
		public void TestCurrentSeasonPost()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2016, 02, 15 ) ) );  // set clock to Feb-2016
			Assert.AreEqual( sut.CurrentSeason(), "2015" );
		}

		[TestMethod]
		public void TestCurrentNextSeason()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2016, 03, 01 ) ) );  // set clock to Mar-2016
			Assert.AreEqual( sut.CurrentSeason(), "2016" );
		}

		[TestMethod]
		public void TestCurrentSeason2016()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2016, 04, 25 ) ) );  // set clock to Apr-2016
			Assert.AreEqual( sut.CurrentSeason(), "2016" );
		}

		[TestMethod]
		public void TestStartOfSeason2017()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2017, 08, 04 ) ) );
			var nextSunday = sut.GetSundayFor( new DateTime( 2017, 8, 4 ) );
			Assert.AreEqual( expected: new DateTime( 2017, 9, 10 ), actual: nextSunday );
		}

		[TestMethod]
		public void TestPreseason()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2015, 03, 16, 12, 0, 0 ) ) );  // set clock to March
			Assert.IsTrue( sut.IsItPreseason() );
			Assert.IsFalse( sut.IsItRegularSeason() );
			Assert.IsFalse( sut.IsItPostSeason() );
		}

		[TestMethod]
		public void TestPreseasonLastSeason()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2014, 03, 16 ) ) );  // set clock to March
			Assert.IsTrue( sut.IsItPreseason() );
			Assert.IsFalse( sut.IsItRegularSeason() );
			Assert.IsFalse( sut.IsItPostSeason() );
		}

		[TestMethod]
		public void TestRegularSeason()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2015, 10, 16, 12, 0, 0 ) ) );  // set clock to October
			Assert.IsFalse( sut.IsItPreseason() );
			Assert.IsTrue( sut.IsItRegularSeason() );
			Assert.IsFalse( sut.IsItPostSeason() );
		}

		[TestMethod]
		public void TestPostSeason()
		{
			var sut = new TimeKeeper( new FakeClock( 
				new DateTime( 2017, 01, 04, 12, 0, 0 ) ) );  // set clock to 4th January 2017
			Assert.IsFalse( sut.IsItPreseason() );
			Assert.IsFalse( sut.IsItRegularSeason() );
			Assert.IsTrue( sut.IsItPostSeason() );
		}

		[TestMethod]
		public void TestIsItPostSeasonInFebruary()
		{
			var sut = new TimeKeeper( new FakeClock( 
				new DateTime( 2017, 02, 14, 12, 0, 0 ) ) );  // set clock to 14 February, 2017
			Assert.IsTrue( sut.IsItPostSeason() );
		}

		[TestMethod]
		public void TestPeakTime()
		{
			var testDateTime = new DateTime( 2014, 09, 04, 3, 42, 0 );
			var sut = new TimeKeeper( null );
			Assert.IsFalse( sut.IsItPeakTime( testDateTime ) );
		}

		[TestMethod]
		public void TestPeakTimeWhen6to1()
		{
			var testDateTime = new DateTime( 2014, 09, 04, 13, 42, 0 );
			var sut = new TimeKeeper( null );
			Assert.IsTrue( sut.IsItPeakTime( testDateTime ) );
		}

		[TestMethod]
		public void TestTimekeeperKnowslastweek()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2015, 11, 17, 12, 0, 0 ) ) );
			Console.WriteLine( "This week is {0}:{1}", sut.CurrentSeason(), sut.CurrentWeek() );
			Console.WriteLine( "Last week is {0}:{1}", sut.CurrentSeason(), sut.PreviousWeek() );
			Assert.IsTrue( sut.PreviousWeek().Equals( "10" ) );
		}

		[TestMethod]
		public void TestSundayDump()
		{
			var sut = new TimeKeeper( new FakeClock( new DateTime( 2017, 08, 06, 0, 0, 0 ) ) );
			var result = sut.DumpSeasonSundays();
			Assert.IsTrue( result.Equals( 21 ) );
		}
	}
}