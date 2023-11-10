using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.Net;

namespace Function
{
    public abstract class BaseFunction
    {
        private readonly IMediator _mediator;

        protected BaseFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<HttpResponseData> PostAsync(HttpRequestData requestData, IRequest request)
        {
            var response = requestData.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await _mediator.Send(request);

            return response;
        }

        protected async Task<HttpResponseData> PostAsync<TResponse>(HttpRequestData requestData, IRequest<TResponse> request)
        {
            var response = requestData.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            TResponse result = await _mediator.Send(request);

            await response.WriteStringAsync(JsonConvert.SerializeObject(result));

            return response;
        }
    }
}
