﻿using System;
using Butler.Interfaces;

namespace Butler.Helpers
{
   public class SystemClock : IClock
   {
      public static DateTime Now()
      {
         return DateTime.Now;
      }

      public int GetMonth()
      {
         var theDate = DateTime.Now;
         var m = theDate.Month;
         return m;
      }

      DateTime IClock.Now
      {
         get { return DateTime.Now; }
      }
   }
}
