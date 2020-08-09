using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRWebApp.Models
{
    public class VacationViewByManagerModel
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Attachment { get; set; }
    }
}