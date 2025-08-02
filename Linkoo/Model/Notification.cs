namespace Linkoo.Model
{
    public class Notification : BaseModel
    {
        public string Message { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } 
        public bool IsRead { get; set; } = false; 
    }
}
