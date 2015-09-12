using System;
using RosterLib.Interfaces;

namespace Gerard.Tests
{
   public class FakeClock : IClock
   {
      public FakeClock( DateTime theDateTime )
      {
         Now = theDateTime;
      }

      public DateTime Now { get; private set; }

      public int GetMonth()
      {
         return Now.Month;
      }
   }
}
