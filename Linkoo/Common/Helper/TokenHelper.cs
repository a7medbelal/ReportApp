using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReportApp.Model;
using ReportApp.Model.Enum;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ReportApp.Common.Helper
{
    public class TokenHelper
    {

        readonly JWTSetting _jwtSetting;
        public TokenHelper(IOptions<JWTSetting> jwtSettings)
        {
            _jwtSetting = jwtSettings.Value;
        }


        public async Task<string> GenerateToken(string userId , Role role, string name )
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetting.SecretKey);
            var issuer = _jwtSetting.Issuer;
            var audience = _jwtSetting.Audience;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim("roleType" , ((int)role).ToString()),
                    new Claim(ClaimTypes.Name, name) 


                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<RefreshToken> GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(7), // مثلاً أسبوع
                CreatedOn = DateTime.UtcNow
            };
        }

    }
}
