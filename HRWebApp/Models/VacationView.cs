using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRWebApp.Models
{
    public class VacationView
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Attachment { get; set; }
        public string Status { get; set; }

        public string RejectReason { get; set; }
    }
}