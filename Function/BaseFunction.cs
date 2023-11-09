using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Function
{
    public abstract class BaseFunction
    {
        private readonly IMediator _mediator;

        protected BaseFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<HttpResponseData> PostResponse(HttpRequestData requestData, IRequest request)
        {
            var response = requestData.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            await _mediator.Send(request);

            return response;
        }

        protected async Task<HttpResponseData> PostResponse<TResponse>(HttpRequestData requestData, IRequest<TResponse> request)
        {
            var response = requestData.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            TResponse result = await _mediator.Send(request);

            await response.WriteStringAsync(JsonConvert.SerializeObject(result));

            return response;
        }
    }
}
