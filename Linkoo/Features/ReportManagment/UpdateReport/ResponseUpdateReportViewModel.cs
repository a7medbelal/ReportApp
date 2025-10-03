namespace ReportApp.Features.ReportManagment.UpdateReport
{
    public class ResponseUpdateReportViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; } 
        public string? FilePath { get; set; }   
    }
}
