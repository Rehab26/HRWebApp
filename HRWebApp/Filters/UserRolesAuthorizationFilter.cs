using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HRWebApp.Helpers;

namespace HRWebApp.Filters
{
    public class UserRolesAuthorizationFilter : AuthorizeAttribute
    {
        private readonly string[] _allowedRoles;
        public UserRolesAuthorizationFilter(params string[] role)
        {
            this._allowedRoles = role;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorize = false;
            var userId = Convert.ToString(httpContext.Session["userID"]);
            if (!string.IsNullOrEmpty(userId))
            {
                var user = HelperMethod.GetUser(int.Parse(userId));
                    foreach (var role in _allowedRoles)
                    {
                        if (role == user.Type.ToString()) return true;
                    }
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Account" },
                    { "action", "unAuthorized " }
                });
        }
    }
}