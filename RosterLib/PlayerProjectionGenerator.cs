using System.Collections.Generic;

namespace RosterLib
{
   /// <summary>
   /// generates the PGMETRIC data
   /// </summary>
   public class PlayerProjectionGenerator : RosterGridReport
   {
      public PipeLine<PlayerGameProjectionMessage> pipeline { get; set; }

      public ICachePlayers PlayerCache { get; set; }

      public PlayerProjectionGenerator( ICachePlayers playerCache )
      {
         PlayerCache = playerCache;
      }

      public void Execute( NFLGame game )
      {
         if ( pipeline == null ) InitialiseThePipeLine();

         var msg = new PlayerGameProjectionMessage {Game = game, PlayerCache = PlayerCache};
         msg.Game.PlayerGameMetrics = new List<PlayerGameMetrics>();
         if (pipeline != null) pipeline.Execute( msg );
      }

      private void InitialiseThePipeLine()
      {
         pipeline = new PipeLine<PlayerGameProjectionMessage>();
         pipeline.Register( msg => new GetGamePrediction( msg ) );
         pipeline.Register( msg => new ClearGameMetrics( msg ) );
         //  this takes most of the time
         pipeline.Register( msg => new PullMetricsFromPrediction( msg ) );
         pipeline.Register( msg => new SavePlayerGameMetrics( msg ) );
      }
   }

   public class PlayerGameProjectionMessage
   {
      public ICachePlayers PlayerCache { get; set; }

      public NFLPlayer Player { get; set; }

      public NFLGame Game { get; set; }

      public NFLResult Prediction { get; set; }

      public PlayerGameMetrics PlayerGameMetrics { get; set; }

      public IPlayerGameMetricsDao Dao { get; set; }

      public override string ToString()
      {
         return Game.GameName();
      }
   }
}