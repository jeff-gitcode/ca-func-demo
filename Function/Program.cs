using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Infrastructure;
using Application;
using Function.Middelwares;

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
    .ConfigureServices(s =>
    {
        s.AddInfrastracture();
        s.AddApplication();
    })
    .Build();

host.Run();
