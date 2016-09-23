using System.Web.Http;

namespace ButlerWebApi
{
   public class TflController : ApiController
   {
      public TflController()
      {
         //  inject dependencies
      }

      //  API methods? RESTfull? or custom?
      public string Get(int id)
      {
         return "Tfl Self Hosted Web Api is online";
      }
   }
}
