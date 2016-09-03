using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GerardWebApp.Controllers
{
   public class PlayersController : ApiController
    {
      static List<PlayerDto> players = InitialisePlayers();

      private static List<PlayerDto> InitialisePlayers()
      {
         var ret = new List<PlayerDto>();
         ret.Add( new PlayerDto { playerId = "GREEA.01", firstName = "A.J.", lastName = "Green" } );
         ret.Add( new PlayerDto { playerId = "BRYADE01", firstName = "Dez", lastName = "Bryant" } );
         return ret;
      }

      public IEnumerable<PlayerDto> Get()
      {
         return players;
      }

      public PlayerDto Get(string id)
      {
         var player = ( from p in players
                        where p.playerId == id
                        select p ).FirstOrDefault();
         return player;
      }

      public void Post([FromBody] PlayerDto p)
      {
         players.Add( p );
      }

   }

   public class PlayerDto
   {
      public string playerId;
      public string firstName;
      public string lastName;
   }
}
