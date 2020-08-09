using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 

namespace HRWebApp.Models
{
    public enum UserType { Employee = 0, Manager = 1 }
    public enum VacationStatus { Pending, Draft, Reject, Approved }
    public enum VacationType { SickLeave, AnnualLeave, ExceptionalLeave, MotherLeave }
}