using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GerardWebApp.Controllers
{
    public class GerardController : ApiController
    {
      public string Get()
      {
         return "Hello from Gerard API at " + DateTime.Now.ToString();
      }
    }
}
