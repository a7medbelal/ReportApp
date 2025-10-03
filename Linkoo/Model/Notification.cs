using System.ComponentModel.DataAnnotations.Schema;

namespace ReportApp.Model
{
    public class Notification : BaseModel
    {
        public string Message { get; set; }
       
        [ForeignKey("AppUser")]
        public Guid AppUserId { get; set; }
        
        public AppUser AppUser { get; set; }

        [ForeignKey("Report")]
        public Guid? ReportId { get; set; }
        public Report? Report { get; set; }

        public bool IsRead { get; set; } = false; 
    }
}
