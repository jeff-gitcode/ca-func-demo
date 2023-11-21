using AutoMapper;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Moq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Azure.Core.Serialization;
using Newtonsoft.Json;

namespace Function.Tests
{
    public class FunctionUnitTestBase
    {
        protected Mock<FunctionContext> _context;
        protected IMapper _mapper;
        protected Mock<HttpRequestData> _request;
        protected Mock<IConfiguration> _mockConfiguration;

        public FunctionUnitTestBase()
        {
            _mockConfiguration = new Mock<IConfiguration>();

            SetupFunctionContext();
            SetupHttpRequestResponse();
        }

        sealed class TestServices : IServiceProvider
        {
            sealed class TestOptions : IOptions<WorkerOptions>
            {
                // WorkerOptions only has one setting: Serializer
                public WorkerOptions Value => new() { Serializer = new JsonObjectSerializer() };
            }

            static readonly IOptions<WorkerOptions> _options = new TestOptions();

            public object? GetService(Type serviceType)
            {
                if (serviceType == typeof(IOptions<WorkerOptions>))
                {
                    return _options;
                }

                return null;
            }
        }

        protected async Task<string> GetResponseBody(HttpResponseData response)
        {
            response.Body.Position = 0;
            StreamReader reader = new StreamReader(response.Body);
            return await reader.ReadToEndAsync();
        }

        protected void SetRequestBody(Mock<HttpRequestData> request, object requestObject)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(requestObject));
            MemoryStream bodyStream = new MemoryStream(byteArray);

            request.Setup(r => r.Body).Returns(bodyStream);
        }

        private void SetupFunctionContext()
        {
            var services = new TestServices();

            _context = new Mock<FunctionContext>();
            _context.SetupProperty(c => c.InstanceServices, services);
        }

        //private void SetupAutoMapper()
        //{
        //    MapperConfiguration config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.AddProfile(new MappingProfile());
        //    });
        //    _mapper = config.CreateMapper();
        //}

        private void SetupHttpRequestResponse()
        {
            _request = new Mock<HttpRequestData>(_context.Object);

            _request.Setup(r => r.CreateResponse()).Returns(() =>
            {
                Mock<HttpResponseData> response = new Mock<HttpResponseData>(_context.Object);
                response.SetupProperty(r => r.Headers, new HttpHeadersCollection());
                response.SetupProperty(r => r.StatusCode);
                response.SetupProperty(r => r.Body, new MemoryStream());
                return response.Object;
            });
        }

        protected Stream SetRequestBody(object requestObject)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(requestObject));
            MemoryStream bodyStream = new MemoryStream(byteArray);

            return bodyStream;
        }

        protected T GetBodyObjectFromResponse<T>(HttpResponseData response)
        {
            string body = string.Empty;
            using (var reader = new StreamReader(response.Body))
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                body = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(body);
        }

    }
}
