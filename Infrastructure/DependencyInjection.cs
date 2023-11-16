using Application.Abstraction;
using Infrastructure.CosmosDB;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Jwt;
using Infrastructure.Authorize;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using Scrutor;
using Application;

// using Microsoft.Extensions.Options.ConfigurationExtensions;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastracture(this IServiceCollection services)
        {
            services.Scan(selector => selector
            .FromAssemblies(
                typeof(ApplicationAssembly).Assembly,
                typeof(InfrastructureAssembly).Assembly)
            .AddClasses(filter=> filter.Where(r=>r.Name.EndsWith("Repository") || r.Name.StartsWith("IJwt")))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime());

            // var assemblies = DependencyContext.Default
            //     .GetDefaultAssemblyNames()
            //     .Where(assembly => assembly.FullName.StartsWith("I"))
            //     .Select(Assembly.Load);

            // services.Scan(
            //     scan =>
            //         scan.FromAssemblies(assemblies)
            //             .AddClasses()
            //             .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            //             .AsMatchingInterface()
            //             .WithScopedLifetime()
            // );

            // services.AddScoped<IJwtOption, JwtOption>();
            // services.AddScoped<IJwtService, JwtService>();
            // services.AddScoped<IAuthService, AuthService>();
            // services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
