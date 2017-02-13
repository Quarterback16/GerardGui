namespace RosterLib.Interfaces
{
   public interface IYahooStatService
   {
      decimal GetStat( string playerId, string season, string week );
      bool IsStat( string playerId, string season, string week );
   }
}
