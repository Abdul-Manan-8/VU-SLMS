namespace VU_SLMS.DTOs
{
    public partial class LeaveRequiredModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime PrevRLeaveFrom { get; set; }
        public DateTime PrevRLeaveTo { get; set; }
        public DateTime ExpcRLeave { get; set; }
        public int? RLeaveCount { get; set; }
        public int? LeavesThisYear { get; set; }
        public int? ExtraLeave { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? Leavecount { get; set; }
        public int? Type { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
