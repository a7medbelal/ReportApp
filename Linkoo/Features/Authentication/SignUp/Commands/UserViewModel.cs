using MediatR.NotificationPublishers;

namespace ReportApp.Features.Authentication.SignUp.Commands
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Password { get; set; }   
        public string email { get; set; }
        public string role { get;  set; }
        public string? Signature { get; set; }
    }
}
