using System.Security.Claims;

namespace Application.Abstraction
{
    public interface IJwtService
    {
        string BuildToken(IEnumerable<Claim> claims);
    }
}
