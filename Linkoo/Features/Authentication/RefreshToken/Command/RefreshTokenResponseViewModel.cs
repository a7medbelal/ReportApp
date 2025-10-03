namespace ReportApp.Features.Authentication.RefreshToken.Command
{
    public class RefreshTokenResponseViewModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}