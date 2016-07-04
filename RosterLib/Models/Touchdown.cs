namespace RosterLib.Models
{
   public class Touchdown
   {
      public string Action { get; set; }
      public int Distance { get; set; }
      public NFLPlayer Scorer { get; set; }
      public NFLPlayer Assisting { get; set; }

      public NFLGame Game { get; set; }

      public string ForTeamCode { get; set; }
      public string AgainstTeamCode { get; set; }

      public override string ToString()
      {
         var s = string.Empty;
         switch (Action)
         {
            case Constants.K_SCORE_TD_PASS:
               s = string.Format("{3}: {2} yd Touchdown pass to {0} from {1} - {4} {5}", Scorer, Assisting, Distance, ForTeamCode, Action, Game.ResultOut( ForTeamCode, true ) );
               break;

            case Constants.K_SCORE_TD_RUN:
               s = string.Format(" Touchdown run by {0}", Scorer);
               s = string.Format("{3}: {2} yd Touchdown run by {0} - {4} {5}", Scorer, Assisting, Distance, ForTeamCode, Action, Game.ResultOut( ForTeamCode, true ) );
               break;

            default:
               s = string.Format(" Touchdown by {0}", Scorer);
               s = string.Format("{3}: {2} yd Touchdown by {0} - {4} {5}", Scorer, Assisting, Distance, ForTeamCode, Action, Game.ResultOut( ForTeamCode, true ) );
               break;
         }
         return s;
      }

   }
}
