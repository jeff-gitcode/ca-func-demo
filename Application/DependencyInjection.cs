using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(typeof(Assembly).Assembly);
        services.AddMediatR(c => c.
            RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    }
}
