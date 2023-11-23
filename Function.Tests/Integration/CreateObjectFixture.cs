using Corvus.Testing.AzureFunctions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Function.Tests.Integration
{
    public class CreateObjectFixture : IAsyncLifetime
    {
        private readonly FunctionsController function;

        public CreateObjectFixture(IMessageSink output)
        {
            ILogger logger = new LoggerFactory().CreateLogger("log");

            this.function = new FunctionsController(logger);
        }

        public int Port => 7265;

        public readonly HttpClient Client = new HttpClient();

        public async Task InitializeAsync()
        {
            await this.function.StartFunctionsInstance(
                @"Function.Users",
                this.Port,
                "..\\..\\..\\..\\Function\\bin\\Debug\\net7.0");

            Client.BaseAddress = new Uri($"http://localhost:{Port}");
        }

        public Task DisposeAsync()
        {
            this.function.TeardownFunctions();
            return Task.CompletedTask;
        }
    }
}
