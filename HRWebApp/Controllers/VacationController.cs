using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using HRWebApp.Filters;
using HRWebApp.Helpers;
using HRWebApp.Models;
namespace HRWebApp.Controllers
{
    public class VacationController : BaseController
    {
        // GET: Vacation
        [UserRolesAuthorizationFilter("Employee")]
        public ActionResult Add(string id)
        {
            if (id != null)
            {
                var vacationModel = HelperMethod.GetVacation(id);
                return View(vacationModel);
            }
            return View();
        }
        //Validation here is done, The method has to be refactor => Todo
        [HttpPost]
        [UserRolesAuthorizationFilter("Employee")]
        public ActionResult Add(VacationManage vacation , string save, string draft)
        {
            if (ModelState.IsValid)
            {
                VacationModelValidation datesValidation = VacationModelValidation.DatesNotValid(vacation.StartDate , vacation.EndDate);
                if (datesValidation != null)
                {
                    ModelState.AddModelError("", datesValidation.ErrorMsg);
                    return View();
                }
                VacationModelValidation fileValidation = VacationModelValidation.FileNotValid(vacation.PostedFile);
                if (fileValidation!=null)
                {
                    ModelState.AddModelError("", fileValidation.ErrorMsg);
                    return View();
                }
                if (!string.IsNullOrEmpty(save))
                {
                        vacation.Status = VacationStatus.Pending;
                }

                if (!string.IsNullOrEmpty(draft))
                {
                        vacation.Status = VacationStatus.Draft;
                }
                var userId = (int)Session["userID"];
                vacation.UserId = userId;
                var url = (vacation.ID != null) ? "api/vacation/update" : "api/vacation/insert";
                if (vacation.PostedFile != null)
                {
                    var  fileName = Path.GetFileNameWithoutExtension(vacation.PostedFile.FileName);
                    var fileExtension = Path.GetExtension(vacation.PostedFile.FileName);
                    fileName = DateTime.Now.ToString("yyyyMMdd") + "-" + fileName.Trim() + fileExtension;
                    var uploadPath = ConfigurationManager.AppSettings["UserAttachment"];
                    vacation.Attachment = uploadPath + fileName;
                    vacation.PostedFile.SaveAs(vacation.Attachment);
                    vacation.Attachment = fileName.Replace(".pdf", "");
                }
                vacation.PostedFile = null;
                var client = HelperMethod.GetHttpClient();
                var stringContent = HelperMethod.GetStringOfObject(vacation);
                var response = client.PostAsync(url, stringContent).Result;
                var sentTo = vacation.Status == VacationStatus.Pending ? "Manager" : "Draft";
                if (response.IsSuccessStatusCode)
                {
                    TempData["Done"] = $"Vacation has been sent to {sentTo}";
                    return View();

                }
            }
            return View();
        }
        [HttpPost]
        [UserRolesAuthorizationFilter("Manager")]
        public JsonResult UpdateStatus(string id , string status , string rejectReason = null)
        {
            var statusVacation = new StatusVacationUpdateModel()
            {
                ID = id,
                Status = status,
                RejectReason = rejectReason
            };
            var client = HelperMethod.GetHttpClient();
            var stringContent = HelperMethod.GetStringOfObject(statusVacation);
            try
            {
                var response = client.PutAsync("api/vacation/UpdateStatus", stringContent).Result;
                if (response.IsSuccessStatusCode)
                    return Json(new {success = true, responseText = "Vacation Updated!"},
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return Json(new {success = false, responseText = "Something went wrong."},
                    JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [UserRolesAuthorizationFilter("Employee")]
        public JsonResult DeleteVacation(string id)
        {
            try
            {
                var client = HelperMethod.GetHttpClient();
                var response = client.DeleteAsync($"api/vacation/delete/{id}").Result;
                if (response.IsSuccessStatusCode)
                    return Json(new {success = true, responseText = "Vacation Deleted!"},
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return Json(new { success = false, responseText = "Something went wrong." },
                JsonRequestBehavior.AllowGet);
        }
        [UserRolesAuthorizationFilter("Employee")]
        public ActionResult Vacations()
        {
            return View();
        }
        [UserRolesAuthorizationFilter("Manager")]
        public ActionResult EmployeeVacation()
        {
            return View("EmployeesVacation");
        }
        [UserRolesAuthorizationFilter("Employee")]
        public ActionResult Info(string id)
        {
            try
            {
                var vacation = HelperMethod.GetVacation(id);
                vacation.ID = id;
                return View(vacation);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return View();
        }
        [HttpGet]
        [UserRolesAuthorizationFilter("Manager" , "Employee")]
        public ActionResult GetFile(string id)
        {
            string file = id + ".pdf";
            string filePath = "~/Attachments/" + file;
            if(id!=null) return File(filePath, "application/pdf");
            Log.Info("Attachment File (Not Found)");
            return HttpNotFound();
        }


        [UserRolesAuthorizationFilter("Manager")]
        public JsonResult GetEmployeeVacations(JQueryDataTableParamModel param)
        {
            var userId = (int) Session["userID"];
            string url = $"api/vacation/GetVacationByManager?id={userId}&search={param.sSearch}";
            try
            {
                var vacations = HelperMethod.GetEmployeeVacation(url);
                var vacationsView = new List<VacationViewByManagerModel>();
                foreach (var vacation in vacations)
                {
                    var vacationView = new VacationViewByManagerModel()
                    {
                        ID = vacation.ID,
                        Email = vacation.Email,
                        FullName = vacation.FullName,
                        StartDate = vacation.StartDate.ToLongDateString(),
                        EndDate = vacation.EndDate.ToLongDateString(),
                        Attachment = vacation.Attachment,
                        Type = Enum.GetName(typeof(VacationType), vacation.Type),
                    };
                    vacationsView.Add(vacationView);
                }
                var displayResult = vacationsView.Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength).ToList();
                var totalRecords = vacationsView.Count();
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
                return Json(new {success = false, responseText = "Something went wrong."},
                    JsonRequestBehavior.AllowGet);
            }
        }

        [UserRolesAuthorizationFilter("Employee")]
        public JsonResult GetVacations(JQueryDataTableParamModel param)
        {
            try
            {
                var userId = (int) Session["userID"];
                string url = $"api/vacation/getAll?id={userId}&search={param.sSearch}";
                var vacations = HelperMethod.GetUserVacation(url);
                var vacationsView = new List<VacationView>();
                foreach (var vacation in vacations)
                {
                    var vacationView = new VacationView()
                    {
                        ID = vacation.ID,
                        StartDate = vacation.StartDate.ToLongDateString(),
                        EndDate = vacation.EndDate.ToLongDateString(),
                        Attachment = vacation.Attachment,
                        Type = Enum.GetName(typeof(VacationType), vacation.Type),
                        Status = Enum.GetName(typeof(VacationStatus), vacation.Status),
                        RejectReason = vacation.RejectReason

                    };
                    vacationsView.Add(vacationView);
                }
                var displayResult = vacationsView.Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength).ToList();
                var totalRecords = vacationsView.Count();

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
                return Json(new { success = false, responseText = "Something went wrong." },
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}