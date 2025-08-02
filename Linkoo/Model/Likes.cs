namespace Linkoo.Model
{
    public class Likes : BaseModel
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } 
        public int PostId { get; set; }
        public Post Post { get; set; } 
    }
}
