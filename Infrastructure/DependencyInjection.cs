using Application.Abstraction;
using Infrastructure.CosmosDB;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Jwt;
using Infrastructure.Authorize;

// using Microsoft.Extensions.Options.ConfigurationExtensions;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastracture(this IServiceCollection services)
        {
            services.AddScoped<IJwtOption, JwtOption>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
