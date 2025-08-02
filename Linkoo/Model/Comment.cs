namespace Linkoo.Model
{
    public class Comment : BaseModel
    {
        public string Content { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } 
        public int PostId { get; set; }
        public Post Post { get; set; }
        
    }
}
