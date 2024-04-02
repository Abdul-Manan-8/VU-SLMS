namespace VU_SLMS.DTOs
{
    public partial class ShortLeaveModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeImage { get; set; }
        public DateTime LeaveDate { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public TimeSpan? TimeDuration { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
