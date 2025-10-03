namespace ReportApp.Features.ReportManagment.AccpetTheReport
{
    public class ResponseApproveReportViewModel
    {
        public Guid ReportId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public long? Number { get; internal set; }
        public string Title { get; internal set; }
        public DateTime? ApprovedAt { get; internal set; }
    }
}