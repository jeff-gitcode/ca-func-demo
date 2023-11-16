using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Infrastructure;
using Application;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.CosmosDB;
using Microsoft.Azure.Cosmos;
using Function;
using Function.Middelwares;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Authorize;
using Infrastructure.Jwt;
using Microsoft.Extensions.Options;

//IConfigurationRoot configuration = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
//    .AddEnvironmentVariables()
//    .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(m =>
    {
        m.UseMiddleware<ExceptionLoggingMiddleware>();
        m.UseMiddleware<AuthMiddleware>();
        m.UseNewtonsoftJson();
    })
    .ConfigureOpenApi()
    .ConfigureAppConfiguration(c =>
    {
        c.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices(
        (context, services) =>
        {
            services.Configure<CosmosDbOptions>(
                options => context.Configuration.GetSection(CosmosDbOptions.CosmosDb).Bind(options)
            );

            services.Configure<JwtOption>(
                options => context.Configuration.GetSection(JwtOption.Jwt).Bind(options)
            );

            //services
            //    .AddOptions<JwtOption>()
            //    .Configure<IConfiguration>((s, c) => c.GetSection(nameof(JwtOption)).Bind(s));

            //services.AddSingleton<IJwtOption>(
            //    x => x.GetRequiredService<IOptions<JwtOption>>().Value
            //);
            //services.AddOptions<CosmosDbOptions>().Configure<IConfiguration>((s, c) =>
            //        c.GetSection(nameof(CosmosDbOptions)).Bind(s));
            var config = context.Configuration
                .GetSection(CosmosDbOptions.CosmosDb)
                .Get<CosmosDbOptions>();

            services.AddInfrastracture();
            services.AddApplication();
            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<CosmosClient>(
                (cc) =>
                {
                    //CosmosClient client = new CosmosClientBuilder(account, primaryKey)
                    //           .WithSerializerOptions(new CosmosSerializationOptions()
                    //           {
                    //               PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    //           })
                    //           .Build();
                    CosmosClient client = new CosmosClient(
                        connectionString: config.PrimaryConnectionString
                    );
                    return client;
                }
            );
        }
    )
    .Build();

host.Run();
