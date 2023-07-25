using System;
using System.Collections.Generic;

namespace VU_SLMS.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Campus { get; set; }
        public string? Designation { get; set; }
        public string? Gender { get; set; }
        public decimal? PhoneNo { get; set; }
        public DateTime JoiningDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
