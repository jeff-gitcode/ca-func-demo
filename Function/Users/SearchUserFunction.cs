using Application.Users;
using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using Application.Users.Queries;
using CleanFunctionApp.Function;
using Function.Middelwares;

namespace Function.Users
{
    public class SearchUserFunction : BaseFunction
    {
        private readonly ILogger _logger;

        public SearchUserFunction(IMediator mediator, ILoggerFactory loggerFactory)
            : base(mediator)
        {
            _logger = loggerFactory.CreateLogger<SearchUserFunction>(); 
        }

        [Function("SearchUserFunction")]
        [Auth("Admin")]
        [OpenApiSecurity("bearer_auth", SecuritySchemeType.Http, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
        [OpenApiOperation(
            operationId: "Get User By Filter",
            tags: new[] { "Customer" }
            //Summary = "Search User.",
            //Description = "Search User.",
            //Visibility = OpenApiVisibilityType.Important
        )]
        [OpenApiRequestBody(
            contentType: "application/json",
            bodyType: typeof(SearchUserDto),
            Required = true,
            Description = "SearchUserFunction"
        )]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(string),
            Summary = "Job list.",
            Description = "List of all the jobs."
        )]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req
        ) => await PostAsync(req, new SearchUsersQuery(req.ValidateAndConvert<SearchUserDto>()));
    }
}
