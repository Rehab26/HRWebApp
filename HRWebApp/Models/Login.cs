

using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HRWebApp.Models
{
    public class Login 
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        
        [Required]
        [MinLength(3, ErrorMessage = "The password should be at least 3 characters length")]
        public string Password { get; set; }
    }
}