using System;
using System.Linq;
using System.Web.Mvc;
using HRWebApp.Filters;
using HRWebApp.Helpers;
using HRWebApp.Models;

namespace HRWebApp.Controllers
{

    public class UserController : BaseController
    {

        // GET: Manager
        [UserRolesAuthorizationFilter("Employee", "Manager")]
        public ActionResult Index()
        {
            try
            {
                var id = (int) Session["userID"];
                var userModel = HelperMethod.GetUser(id);
                return View(userModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return HttpNotFound();
            }
            
        }
        [HttpGet]
        [UserRolesAuthorizationFilter("Employee", "Manager")]
        public ActionResult Edit()
        {
            try
            {
                var id = (int) Session["userID"];
                var userModel = HelperMethod.GetUser(id);
                return View(userModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return HttpNotFound();
            }
            
        }
        [UserRolesAuthorizationFilter( "Manager")]
        public ActionResult EditByManager(int id)
        {
            try
            {
                var userModel = HelperMethod.GetUser(id);
                return View(userModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return HttpNotFound();
            }
            
        }

        [HttpPost]
        [UserRolesAuthorizationFilter("Employee", "Manager")]
        public ActionResult EditUser(User user)
        {
            try
            {

                var isUpdated = HelperMethod.UpdateUser(user);
                var updatedUser = HelperMethod.GetUser(user.ID);
                if (isUpdated) return View("Profile", updatedUser); 
                return View("Edit");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return HttpNotFound();
        }

        [UserRolesAuthorizationFilter("Manager")]
        public ActionResult EditEmployee(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = HelperMethod.GetHttpClient();
                    var stringContent = HelperMethod.GetStringOfObject(user);
                    var response = client.PostAsync($"api/user/UpdateEmployee", stringContent).Result;
                    if (response.IsSuccessStatusCode) return View("Employees");
                }
            } catch(Exception ex) {Log.Error(ex.Message);}
            return View("EditByManager");
        }
        [UserRolesAuthorizationFilter("Employee", "Manager")]
        public new ActionResult Profile()
        {
            try
            {
                int id = (int) Session["userID"];
                var userModel = HelperMethod.GetUser(id);
                return View(userModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return HttpNotFound();
        }
        [UserRolesAuthorizationFilter("Manager")]
        public ActionResult EmployeeProfile(int id)
        {
            try
            {
                var userModel = HelperMethod.GetUser(id);
                return View("Profile", userModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return View("Error");

        }
        [UserRolesAuthorizationFilter("Manager")]
        public ActionResult Employees()
        {
            return View();
        }
        [HttpPost]
        [UserRolesAuthorizationFilter("Manager")]
        public ActionResult Delete(int id)
        {
            try
            {
                var client = HelperMethod.GetHttpClient();
                var response = client.DeleteAsync($"api/user/delete/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    Json(new {success = true, responseText = "Employee Deleted!"},
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return Json(new { success = false, responseText = "Something went wrong!." },
                JsonRequestBehavior.AllowGet);
        }

        [UserRolesAuthorizationFilter("Manager")]
        public JsonResult GetEmployees(JQueryDataTableParamModel param )
        {
            try
            {
                var userId = (int) Session["userID"];
                var url = $"api/user/getEmployees?id={userId}&search={param.sSearch}";
                var employees = HelperMethod.GetUsers(url);
                //Web api, TEST HERE
                var userViews = employees.ToList();
                var displayResult = userViews.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var totalRecords = userViews.Count();

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = displayResult,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return Json(new {success = false, responseText = "Something went wrong."},
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [UserRolesAuthorizationFilter("Manager")]
        public ActionResult Create()
        {
            var user = new User { ManagerID = (int)Session["userID"]};
            return View(user);
        }
        [HttpPost]
        [UserRolesAuthorizationFilter("Manager")]
        public ActionResult Create(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var managerId = (int) Session["userID"];
                    user.ManagerID = managerId;
                    var client = HelperMethod.GetHttpClient();
                    var stringContent = HelperMethod.GetStringOfObject(user);
                    var response = client.PostAsync("api/user/insert", stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SucessMessage"] = $"Employee {user.FullName} created successfully";
                        return View(user);
                    }

                    return View(user);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            ModelState.AddModelError("", "Server error, please try again");
            return View();

        }

    }
}