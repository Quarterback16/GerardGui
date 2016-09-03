using System.Web.Mvc;

namespace GerardWebApp.Controllers
{
   public class HomeController : Controller
   {
      public ActionResult Index()
      {
         ViewBag.Title = "Home Page";

         return View();
      }
   }
}
