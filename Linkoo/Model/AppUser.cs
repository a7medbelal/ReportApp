using ReportApp.Model.Enum;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ReportApp.Model
{
    public class AppUser : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Fname{ get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        
        public string? ProfilePicture { get; set; }
        
        [MaxLength(250)]         
        public string? Bio { get; set; }
        
        public string? HashedPassword { get; set; }
        
        public string? SaltPassword { get; set; }

        
        public int? OrgNumber { get; set; }

        public string? PersidentSignature { get; set; }

        public Role role { get; set; }     

        public ICollection<RefreshToken> refreshToken { get; set; } = new Collection<RefreshToken>();
 
        public ICollection<Report> reports { get; set; } = new List<Report>();
    }
}
