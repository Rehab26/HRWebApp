using HRWebApp.Helpers;
using HRWebApp.Models;
using System.Web.Mvc;
using System.Web.Security;
using HRWebApp.Filters;

namespace HRWebApp.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }

}