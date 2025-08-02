namespace Linkoo.Model
{
    public class Post : BaseModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public ICollection<Tags>tags { get; set; } = new List<Tags>();
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
