using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRWebApp.Models
{
    public class EmployeeVacation
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public VacationType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public VacationStatus Status { get; set; }
        public string Attachment { get; set; }
        public string RejectReason { get; set; }
    }
}