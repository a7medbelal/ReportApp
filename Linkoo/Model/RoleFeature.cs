using ReportApp.Model.Enum;
using System.Diagnostics.Contracts;

namespace ReportApp.Model
{
    public class RoleFeature : BaseModel
    {
      public  Role Role { get; set; } 

      public AppFeature Feature { get; set; }
    }
}
