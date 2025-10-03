using Autofac;

namespace ReportApp.Features.ReportManagment.GetReportById.Query
{
    public class GetReportsByidDTO
    {
        public string OwnerName { get; set; }   
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? ApprovedAt  { get; set; }
        public string Status { get; set; }  
        public  long? Num { get; set; }
        public String ?  filePath { get; set; }    

    }
}