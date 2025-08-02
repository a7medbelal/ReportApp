using System.Collections.ObjectModel;

namespace Linkoo.Model
{
    public class AppUser : BaseModel
    {
        public string Name{ get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }
        public string Provider { get; set; }

        public Collection<Post> Posts { get; set; }
    }
}
