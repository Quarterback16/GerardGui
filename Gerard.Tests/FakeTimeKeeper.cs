using System.Globalization;
using RosterLib;
using RosterLib.Interfaces;
using System;


namespace Gerard.Tests
{
   public class FakeTimeKeeper : IKeepTheTime
   {
      public string Season { get; set; }

      public string Week { get; set; }

      public bool _isItPreseason { get; set; }

      public bool _isPeakTime { get; set; }

	   public DateTime TheDateTime { get; set; }

      public FakeTimeKeeper()
      {
         Season = "2015";
         _isItPreseason = true;
         _isPeakTime = false;
	      TheDateTime = DateTime.Now;
      }

		public FakeTimeKeeper( DateTime theDate )
		{
			Season = "2015";
			_isItPreseason = true;
			_isPeakTime = false;
			TheDateTime = theDate;
		}

      public FakeTimeKeeper( bool isPreSeason, bool isPeakTime )
      {
         Season = "2015";
         Week = isPreSeason ? "00" : "01";

         _isItPreseason = isPreSeason;
         _isPeakTime = isPeakTime;
			TheDateTime = DateTime.Now;
      }

      public FakeTimeKeeper( string season )
      {
         Season = season;
         Week = "00";
      }

      public FakeTimeKeeper(string season, string week )
      {
         Season = season;
         Week = week;
      }

      public bool IsItWednesdayOrThursday( DateTime focusDate )
      {
         return false;
      }

      public bool IsItWednesday(DateTime focusDate)
      {
         return false;
      }
      
      public bool IsItOffSeason()
      {
         return true;
      }

      public bool IsItPreseason()
      {
         return _isItPreseason;
      }

      public bool IsItPostSeason()
      {
         return false;
      }

      public bool IsItRegularSeason()
      {
         return false;
      }

      public bool IsItQuietTime()
      {
         return false;
      }

      public bool IsItPeakTime()
      {
			return _isPeakTime;
      }

      public DateTime GetDate()
      {
			return TheDateTime;
      }

      public bool IsDateDaysOld(int daysOld, DateTime theDate)
      {
         return true;
      }

      public string CurrentSeason( DateTime focusDate )
      {
         return Season;
      }

      public string PreviousWeek()
      {
         var currentWeek = CurrentWeek(CurrentDateTime());
         var previousWeek = currentWeek - 1;
         return string.Format("{0:0#}", previousWeek);
      }

      public string PreviousSeason(DateTime focusDate)
      {
         throw new NotImplementedException();
      }

      public string PreviousSeason()
      {
         var ps = Int32.Parse(Season) - 1;
         return ps.ToString( CultureInfo.InvariantCulture );
      }

      public int CurrentWeek( DateTime focusDate )
      {
         return Int32.Parse( Week );
      }

      public bool IsItFridaySaturdayOrSunday( DateTime focusDate )
      {
         return focusDate.DayOfWeek == DayOfWeek.Friday || focusDate.DayOfWeek == DayOfWeek.Saturday || focusDate.DayOfWeek == DayOfWeek.Sunday;
      }

	   public DateTime CurrentDateTime()
	   {
		   return TheDateTime;
	   }

		public DateTime GetSundayFor( DateTime when )
		{
			var theSeason = Utility.SeasonFor( when );
			var theSunday = Utility.TflWs.GetSeasonStartDate( theSeason );
			if ( when <= theSunday )
				return theSunday;
			for ( var i = 1; i < 16; i++ )
			{
				var sunday = theSunday.AddDays( i * 7 );
				if ( when > sunday ) continue;
				theSunday = sunday;
				break;
			}
			return theSunday;
		}
   }
}
