namespace Reportapp.Features.Authentication.Login.Login
{
    public class LoginDTO
    {
       public string Token { get; set; } 
       public  Guid  ID { get; set; }
       public string name { get;  set; }
       public string Role { get;  set; }
       public string refreshToken { get; set; }
         public DateTime refreshTokenExpiryTime { get; set; }
    }
}