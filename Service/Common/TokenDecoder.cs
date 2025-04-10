using FirebaseAdmin.Auth;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Common
{
    public class TokenDecoder
    {
        public static (string email, string fullname) DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new ArgumentException("Invalid token.");
            }

            // Extract email claim
            var emailClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (emailClaim == null)
            {
                throw new ArgumentException("Email claim not found in token.");
            }

            // Extract fullname claim (assuming it's a custom claim)
            var fullnameClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;

            return (emailClaim, fullnameClaim);
        }

        public static string GetEmailFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new ArgumentException("Invalid token.");
            }

            // Extract email claim
            var emailClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (emailClaim == null)
            {
                throw new ArgumentException("Email claim not found in token.");
            }

            return emailClaim;
        }

        public static string GetRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new ArgumentException("Invalid token.");
            }

            // Extract email claim


            // Extract role claim 
            var roleClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
            if (roleClaim == null)
            {
                throw new ArgumentException("Role claim not found in token.");
            }

            return roleClaim;
        }

        public static bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new ArgumentException("Invalid token.");
            }
            var expClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (expClaim == null)
            {
                throw new ArgumentException("Expiration claim not found in token.");
            }

            if (!long.TryParse(expClaim, out long exp))
            {
                throw new ArgumentException("Expiration claim is not a valid integer.");
            }
            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
            return DateTime.UtcNow > expirationDate;
        }
    }
}
