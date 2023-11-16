using Infrastructure.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authorize
{
    public class AuthService : IAuthService
    {
        private readonly JwtOption _jwtOption;
        private ClaimsPrincipal? Claims { get; set; }

        public AuthService(IOptionsMonitor<JwtOption> jwtOption)
        {
            _jwtOption = jwtOption.CurrentValue;
        }

        public bool CheckAuthorization(string bearerToken, string[]? roles = null)
        {
            var token = bearerToken.Replace("Bearer ", string.Empty);

            SymmetricSecurityKey issuerSigningKey = new(Encoding.ASCII.GetBytes(_jwtOption.Secret));

            SigningCredentials credentals = new(issuerSigningKey, SecurityAlgorithms.HmacSha256);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = issuerSigningKey,
                ValidateIssuer = true,
                ValidIssuer = _jwtOption.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtOption.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            Claims = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out _);

            if (roles != null)
            {
                var role = Claims?.FindFirst(ClaimTypes.Role)?.Value;

                if (role == null) return false;

                return roles.Contains(role, StringComparer.InvariantCultureIgnoreCase);
            }

            return false;
        }

        public string GetSpecificClaim(string claimType)
        {
            return Claims?.FindFirst(claimType)?.Value!;
        }
    }

    public interface IAuthService
    {
        bool CheckAuthorization(string bearerToken, string[]? roles = null);
        string GetSpecificClaim(string claimType);
    }
}
