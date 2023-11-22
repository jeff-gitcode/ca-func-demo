using Function.Users;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Infrastructure.Authorize;
using Infrastructure.CosmosDB;
using Infrastructure.Jwt;
using Microsoft.Azure.Cosmos;
using Infrastructure;
using Application;
using Function.Middelwares;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;

namespace Function.Tests.Integration.Users
{
    public class TestProgram
    {
        public TestProgram()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(m =>
                {
                    m.UseMiddleware<ExceptionLoggingMiddleware>();
                    m.UseMiddleware<AuthMiddleware>();
                    m.UseNewtonsoftJson();
                })
                .ConfigureAppConfiguration(c =>
                {
                    c.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(
                    (context, services) =>
                    {
                        services.Configure<CosmosDbOptions>(
                            options =>
                                context.Configuration
                                    .GetSection(CosmosDbOptions.CosmosDb)
                                    .Bind(options)
                        );

                        services.Configure<JwtOption>(
                            options => context.Configuration.GetSection(JwtOption.Jwt).Bind(options)
                        );

                        var config = context.Configuration
                            .GetSection(CosmosDbOptions.CosmosDb)
                            .Get<CosmosDbOptions>();

                        services.AddInfrastracture();
                        services.AddApplication();
                        services.AddScoped<IAuthService, AuthService>();

                        services.AddSingleton<CosmosClient>(
                            (c) =>
                            {
                                CosmosClient client = new CosmosClient(
                                    connectionString: config.PrimaryConnectionString
                                );
                                return client;
                            }
                        );

                        services.AddTransient<RegisterUserFunction>();
                        services.AddTransient<LoginUserFunction>();
                        services.AddTransient<SearchUserFunction>();
                    }
                )
                // .ConfigureWebJobs(builder => builder.UseWebJobsStartup(typeof(Program), new WebJobsBuilderContext(), NullLoggerFactory.Instance))
                .Build();

            ServiceProvider = host.Services;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
