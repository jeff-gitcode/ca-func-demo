using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.Net;
using System.Text.Json;

namespace Function.Middelwares
{
    public class ExceptionLoggingMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                List<string> trace = new();
                Exception? tracer = ex;
                while (tracer is not null)
                {
                    trace.Add(tracer!.Message);
                    tracer = tracer!.InnerException;
                }

                // return this response with status code 500
                var requestData = await context.GetHttpRequestDataAsync();
                if (requestData != null)
                {
                    var response = requestData.CreateResponse(HttpStatusCode.InternalServerError);
                    await response.WriteAsJsonAsync(new
                    {
                        success = false,
                        errors = JsonSerializer.Serialize(trace.ToArray())
                    },
                        response.StatusCode);
                    context.GetInvocationResult().Value = response;
                }
            }
        }
    }
}
