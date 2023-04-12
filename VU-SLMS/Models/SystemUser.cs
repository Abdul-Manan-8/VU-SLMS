using System;
using System.Collections.Generic;

namespace VU_SLMS.Models
{
    public partial class SystemUser
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int? Type { get; set; }
    }
}
