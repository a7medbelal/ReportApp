namespace ReportApp.Model
{
    public class BaseModel
    {
        public Guid  Id { get; set; } 
        
        public DateTime CreatedAt { get; set; } = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);        
        public DateTime? UpdatedAt { get; set; } 
       
        public bool IsDeleted { get; set; } = false;
        
        public string Createdby { get; set; } 
      
        public string ?UpdatedBy { get; set; }

    }
}
