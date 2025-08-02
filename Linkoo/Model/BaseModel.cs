namespace Linkoo.Model
{
    public class BaseModel
    {
       public int Id { get; set; }  
       public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
       public bool IsDeleted { get; set; } = false;
       public int CreatedBy { get; set; } 
       public int ?UpdatedBy { get; set; }

    }
}
