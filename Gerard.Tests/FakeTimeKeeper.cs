using System.Globalization;
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

      public FakeTimeKeeper()
      {
         Season = "2015";
         _isItPreseason = true;
         _isPeakTime = false;
      }

      public FakeTimeKeeper( bool isPreSeason, bool isPeakTime )
      {
         Season = "2015";
         Week = isPreSeason ? "00" : "01";

         _isItPreseason = isPreSeason;
         _isPeakTime = isPeakTime;
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
         return false;
      }

      public DateTime GetDate()
      {
         throw new NotImplementedException();
      }

      public bool IsDateDaysOld(int daysOld, DateTime theDate)
      {
         return true;
      }

      public string CurrentSeason( DateTime focusDate )
      {
         return Season;
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


      public bool IsItWednesday(DateTime focusDate)
      {
         return focusDate.DayOfWeek == DayOfWeek.Wednesday;
      }
   }
}
