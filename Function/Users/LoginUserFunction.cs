using System.Net;
using Application.Users;
using Application.Users.Commands;
using Application.Users.Queries;
using CleanFunctionApp.Function;
using Domain;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Function.Users
{
    public class LoginUserFunction : BaseFunction
    {
        private readonly ILogger _logger;

        public LoginUserFunction(IMediator mediator, ILoggerFactory loggerFactory)
            : base(mediator)
        {
            _logger = loggerFactory.CreateLogger<LoginUserFunction>();
        }

        [Function("LoginUserFunction")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        // [OpenApiSecurity(
        //     "function_key",
        //     SecuritySchemeType.ApiKey,
        //     Name = "code",
        //     In = OpenApiSecurityLocationType.Query
        // )]
        [OpenApiRequestBody("application/json", typeof(UserDto), Description = "LoginUserFunction")]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "text/plain",
            bodyType: typeof(Customer),
            Description = "Token"
        )]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var model = req.ValidateAndConvert<UserDto>();

            return await PostAsync(req, new LoginUserQuery(model));
            //var response = req.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            //response.WriteString("Welcome to Azure Functions!");

            //return response;
        }
    }
}
