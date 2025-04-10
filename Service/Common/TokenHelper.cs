using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Common
{
    public class TokenHelper
    {
        private static readonly TokenHelper _instance = new TokenHelper();

        private TokenHelper() { }

        public static TokenHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        public string CreateToken(IEnumerable<Claim> authClaims, IConfiguration configuration)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}