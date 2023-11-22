using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Function.Tests.Integration
{
    public static class JwtTokenProvider
    {
        public static string Issuer { get; } = "Sample_Auth_Server";
        public static SecurityKey SecurityKey { get; } =
            new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes("This_is_a_super_secure_key_and_you_know_it")
            );
        public static SigningCredentials SigningCredentials { get; } =
            new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        internal static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
    }

    public class TestJwtToken
    {
        public List<Claim> Claims { get; } = new();
        public int ExpiresInMinutes { get; set; } = 30;

        public TestJwtToken WithRole(string roleName)
        {
            Claims.Add(new Claim(ClaimTypes.Role, roleName));
            return this;
        }

        public string Build()
        {
            var token = new JwtSecurityToken(
                JwtTokenProvider.Issuer,
                JwtTokenProvider.Issuer,
                Claims,
                expires: DateTime.Now.AddMinutes(ExpiresInMinutes),
                signingCredentials: JwtTokenProvider.SigningCredentials
            );
            return JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
