using System;

namespace Butler.Interfaces
{
   public interface IClock
   {
      DateTime Now { get; }

      int GetMonth();
   }
}
