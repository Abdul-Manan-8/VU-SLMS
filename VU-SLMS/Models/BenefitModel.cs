using System;
using System.Collections.Generic;

namespace VU_SLMS.Models
{
    public partial class BenefitModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? EmployeeId { get; set; }
        public int? EmployeeName { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
