using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRWebApp.Models
{
    public class UserView
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public int EarnSickLeave { get; set; }
        public int EarnAnnualLeave { get; set; }
    }
}