using RosterLib.Interfaces;

namespace RosterLib
{
   public class GameProjectionsReport : RosterGridReport
   {
      public NflSeason NflSeason { get; private set; }

      public GameProjectionsReport(IKeepTheTime timekeeper)
      {
         Name = "Game Projections Report";
         NflSeason = new NflSeason(timekeeper.CurrentSeason(), loadGames:true, loadDivisions:false);
      }

      public override void RenderAsHtml()
      {
         foreach ( var game in NflSeason.GameList )
         {
            game.WriteProjection();
#if DEBUG
            if (game.WeekNo > 1 )break;
#endif
         }
      }
   }
}
