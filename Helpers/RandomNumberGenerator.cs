using System;

namespace Helpers
{
   public static class RandomNumberGenerator
   {
      public static Random RNG = new Random( 0 );

      public static int GetRandomIntBetween( int lowerInclusive, int higherInclusive )
      {
         return RNG.Next( lowerInclusive, higherInclusive );
      }   
   }
}
