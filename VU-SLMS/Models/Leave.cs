using System;
using System.Collections.Generic;

namespace VU_SLMS.Models
{
    public partial class Leave
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EmpolyeeName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
