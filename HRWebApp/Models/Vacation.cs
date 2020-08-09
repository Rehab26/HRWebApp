using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace HRWebApp.Models
{
    public class Vacation
    {
        public string ID { get; set; }
        [Required]
        public VacationType Type { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date),DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public VacationStatus Status { get; set; }

        [StringLength(200)]
        public string RejectReason { get; set; }
        [DisplayName("Attachment File")]
        [FileExtensions(Extensions = "pdf", ErrorMessage = "Please select an pdf file.")]
        public string Attachment { get; set; }
        public HttpPostedFileBase PostedFile { get; set; }

    }
}