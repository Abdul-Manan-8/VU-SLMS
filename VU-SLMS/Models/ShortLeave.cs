using System;
using System.Collections.Generic;

namespace VU_SLMS.Models
{
    public partial class ShortLeave
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LeaveDate { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public TimeSpan? TimeDuration { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
