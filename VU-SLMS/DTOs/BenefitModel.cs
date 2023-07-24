namespace VU_SLMS.DTOs
{
    public partial class BenefitModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Amount { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime DateOfIssue { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
