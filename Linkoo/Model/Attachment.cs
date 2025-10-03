using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportApp.Model
{
    public class Attachment : BaseModel
    {
        [ForeignKey("Report")]
        [Required]  
        public Guid ReportId { get; set; }
   
        public Report Report { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public long Size { get; set; }
    }
}
