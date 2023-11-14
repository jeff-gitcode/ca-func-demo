using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Infrastructure;
using Application;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.CosmosDB;
using Microsoft.Azure.Cosmos;
using Function;

//IConfigurationRoot configuration = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
//    .AddEnvironmentVariables()
//    .Build();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(
    //w =>
    //{
    //    //w.UseMiddleware<ExceptionLoggingMiddleware>();
    //}
    )
    .ConfigureOpenApi()
    .ConfigureAppConfiguration(c =>
    {
        c.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices(
        (context, services) =>
        {
            services.Configure<CosmosDbOptions>(options => context.Configuration.GetSection(CosmosDbOptions.CosmosDb).Bind(options));
            //services.AddOptions<CosmosDbOptions>().Configure<IConfiguration>((s, c) =>
            //        c.GetSection(nameof(CosmosDbOptions)).Bind(s));
            var config = context.Configuration
                .GetSection(CosmosDbOptions.CosmosDb)
                .Get<CosmosDbOptions>();

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

            services.AddInfrastracture();
            services.AddApplication();
        }
    )
    .Build();

host.Run();
