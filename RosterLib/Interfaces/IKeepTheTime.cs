using System;

namespace RosterLib.Interfaces
{
   public interface IKeepTheTime
   {
      string Season { get; set; }

      string Week { get; set; }

      bool IsItPreseason();

      bool IsItPostSeason();

      bool IsItRegularSeason();

      bool IsItPeakTime();

      DateTime GetDate();

      bool IsDateDaysOld(int daysOld, DateTime theDate);

      bool IsItWednesdayOrThursday( DateTime focusDate );

      string CurrentSeason( DateTime focusDate );

      string PreviousSeason( DateTime focusDate);

      string PreviousSeason();

      int CurrentWeek( DateTime focusDate );

      bool IsItFridaySaturdayOrSunday( DateTime dateTime );
   }
}