using System.Net;
using Application.Users;
using Application.Users.Commands;
using CleanFunctionApp.Function;
using Domain;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Function.Users
{
    public class RegisterUserFunction : BaseFunction
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

        public RegisterUserFunction(IMediator mediator, ILoggerFactory loggerFactory)
            : base(mediator)
        {
            _logger = loggerFactory.CreateLogger<RegisterUserFunction>();
        }

        [OpenApiOperation(operationId: "Run", tags: new[] { "Customer" })]
        // [OpenApiSecurity(
        //     "function_key",
        //     SecuritySchemeType.ApiKey,
        //     Name = "code",
        //     In = OpenApiSecurityLocationType.Query
        // )]
        [OpenApiRequestBody(
            "application/json",
            typeof(UserDto),
            Description = "RegisterUserFunction"
        )]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "text/plain",
            bodyType: typeof(Customer),
            Description = "The OK response"
        )]
        [Function("RegisterUserFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var model = req.ValidateAndConvert<UserDto>();

                var result = await PostAsync(req, new RegisterUserCommand(model));

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
