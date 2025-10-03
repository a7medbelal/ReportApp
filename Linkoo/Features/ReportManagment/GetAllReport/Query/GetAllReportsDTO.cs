using Autofac;

namespace ReportApp.Features.ReportManagment.GetAllReport.Query
{
    public class GetAllReportsDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? ApprovedAt  { get; set; }
        public string Status { get; set; }  
        public  long? Num { get; set; }
    }
}