using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using HRWebApp.Helpers;

namespace HRWebApp.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string JobTitle { get; set; }
        public UserType Type { get; set; }
        public int EarnSickLeave { get; set; }
        public int EarnAnnualLeave { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "The password should be at least 3 characters length")]
        public string Password { get; set; }
        public int ManagerID { get; set; }
    }
    
}