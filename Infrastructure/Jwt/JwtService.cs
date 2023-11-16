using Application.Abstraction;
using Infrastructure.CosmosDB;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtOption _option;

        public JwtService(IOptionsMonitor<JwtOption> option)
        {
            this._option = option.CurrentValue;
        }

        public string BuildToken(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey key = new(Encoding.ASCII.GetBytes(_option.Secret));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt =
                new(
                    claims: claims,
                    issuer: _option.Issuer,
                    expires: DateTime.Now.AddMinutes(_option.Expires),
                    audience: _option.Audience,
                    notBefore: DateTime.Now,
                    signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }
    }
}
