namespace Linkoo.Model
{
    public class Tags : BaseModel
    {
        public string Name { get; set; }  
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}