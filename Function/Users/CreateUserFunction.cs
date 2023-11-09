using System.Net;
using Application.Users.Commands;
using CleanFunctionApp.Function;
using Domain;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Function.Users
{
    public class CreateUserFunction : BaseFunction
    {
        //private readonly ILogger _logger;

        //public CreateUserFunction(ILoggerFactory loggerFactory)
        //{
        //    _logger = loggerFactory.CreateLogger<CreateUserFunction>();
        //}

        //[Function("CreateUserFunction")]
        //public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        //{
        //    _logger.LogInformation("C# HTTP trigger function processed a request.");

        //    var response = req.CreateResponse(HttpStatusCode.OK);
        //    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        //    response.WriteString("Welcome to Azure Functions!");

        //    return response;
        //}
        private readonly ILogger _logger;

        public CreateUserFunction(IMediator mediator, ILoggerFactory loggerFactory) : base(mediator)
        {
            _logger = loggerFactory.CreateLogger<CreateUserFunction>();
        }

        [Function("CreateUserFunction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            return await PostResponse(req, new CreateUserCommand(req.Convert<User>()));
        }

    }
}
