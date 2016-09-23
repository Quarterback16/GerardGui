using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace ButlerWebApi
{
   class Program
   {
      static void Main( string[] args )
      {
         // config info
         var config = new HttpSelfHostConfiguration(
            new Uri( "http://localhost:50651" ) );

         // use the default routing
         config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional } );

         //  set ups a server which creates the WCF channel stack responsible for interaction
         using ( HttpSelfHostServer server = new HttpSelfHostServer( config ) )
         {
            server.OpenAsync().Wait();
            Console.WriteLine( "Press Enter to quit." );
            Console.ReadLine();
         }
      }
   }
}
