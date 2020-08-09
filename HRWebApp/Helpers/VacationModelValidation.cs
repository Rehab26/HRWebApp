using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRWebApp.Helpers
{
    //Todo move to model
    public class VacationModelValidation
    {
        public bool IsNotValid { get; set; }
        public string ErrorMsg { get; set; }

        public static VacationModelValidation DatesNotValid(DateTime start, DateTime end)
        {
            if (start <= DateTime.Now) return new VacationModelValidation
            {
                IsNotValid = true, ErrorMsg = "Start date should starts with tomorrow date, or more"
            };
            if (start > end) return new VacationModelValidation
            {
                IsNotValid = true, ErrorMsg = "Start date must be earlier than end date"
            };
            return null;
        }

        public static VacationModelValidation FileNotValid(HttpPostedFileBase file)
        {
            if (file != null && ! file.FileName.Contains("pdf"))
            {
                return new VacationModelValidation
                {
                    IsNotValid = true,
                    ErrorMsg = "Please only select pdf file"
                };
            }
            return null;
        }

    }
}