using log4net;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace HRWebApp.Controllers
{
    
    public class BaseController : Controller
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));
        // GET: Base
        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            if (filterContext.Controller.GetType().Name == "AccountController" && filterContext.ActionDescriptor.ActionName == "Login")
            {
                if (Session == null)
                {
                    return;
                }
                Session.Clear();
            }
            else
            {
                if (Session == null)
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                    return;
                }
            }
            base.OnAuthentication(filterContext);
        }
    }
}