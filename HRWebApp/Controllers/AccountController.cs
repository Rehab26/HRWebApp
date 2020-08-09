using System;
using System.Web.Mvc;
using HRWebApp.Helpers;
using HRWebApp.Models;

namespace HRWebApp.Controllers
{
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["ID"] = null;
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult UnAuthorized()
        {
            return View();
        }
        [HttpPost]
        [Route("Login")]

        public ActionResult Login(Login login)
        {
            ViewBag.Message = "Please login to your account";
            if (ModelState.IsValid)
            {
                var client = HelperMethod.GetHttpClient();
                var stringContent = HelperMethod.GetStringOfObject(login);
                //Exception here
                try
                {
                    var response = client.PostAsync("api/user/Login", stringContent).Result;
                    var httpResponseContent = response.Content.ReadAsStringAsync().Result;
                    var user = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<User>(httpResponseContent);
                    if (user == null)
                    {
                        Log.Info($"Wrong try login with {login.Email} and {login.Password}");
                        ModelState.AddModelError("", "Invalid login attempt, try with correct email or password");
                        return View(login);
                    }
                    Session["userID"] = user.ID;
                    Session["user"] = user;
                    return RedirectToAction("index", "User");
                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                    {
                        if (e.InnerException.InnerException != null)
                        {
                            Log.Error(e.InnerException.InnerException.Message);
                        }
                    }
                    else Log.Error(e.Message);
                    ModelState.AddModelError("", "Server error, please try later");
                    return View(login);
                }
            }
            return View(login);

        }
    }
}