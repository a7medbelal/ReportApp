using ReportApp.Model.Enum;

namespace ReportApp.Model
{
    public class Report : BaseModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Subject { get; set; }

        public long? Number { get; set; }

        public ReportStatus  Status { get; set; } = ReportStatus.Pending;

        public string? FilePath { get; set; }    


        public Guid CreatedById { get; set; }
        
        public AppUser CreatedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
