using Autofac;

namespace ReportApp.Features.ReportManagment.GetAllReportForPresdientApproved.Query
{
    public class GetAllReportsForPreisdentApprovedViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? ApprovedAt  { get; set; }
        public string Status { get; set; }  
        public  long? Num { get; set; }
    }
}