using Application.Users;
using Application.Users.Commands;
using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using Function.Users;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Function.Tests.Users;

public class RegisterUserFunctionTests : FunctionUnitTestBase
{
    private readonly Mock<ILoggerFactory> _logger;
    private readonly Mock<IMediator> _mediator;

    private readonly RegisterUserFunction _sut;

    public RegisterUserFunctionTests()
    {
        var loggerMock = new Mock<ILogger<RegisterUserFunction>>();
        _logger = new Mock<ILoggerFactory>();
        _logger.Setup(r => r.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

        _mediator = new Mock<IMediator>();

        _sut = new RegisterUserFunction(_mediator.Object, _logger.Object);
    }

    [Theory, CustomAutoData]
    public async Task RegisterUserFunctionTests_WhenRun_ShouldReturns(
        [Frozen] Mock<HttpRequestData> httpRequestData,
        [Frozen] Mock<HttpResponseData> httpResponseData,
        Customer customer,
        UserDto userDto
    )
    {
        //httpResponseData.SetupGet(x => x.StatusCode)
        //       .Returns(HttpStatusCode.OK);

        //httpRequestData.Setup(x => x.CreateResponse())
        //        .Returns(httpResponseData.Object);

        //// var functionContext = SetupFunctionContext();
        //httpRequestData.Setup(x => x.Body).Returns(SetRequestBody(userDto));
        //httpRequestData.Setup(x => x.FunctionContext).Returns(functionContext);

        userDto.Email = "test@test.com";
        SetRequestBody(_request, userDto);

        _mediator
            .Setup(r => r.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var response = await _sut.Run(_request.Object);

        var actual = GetBodyObjectFromResponse<Customer>(response);

        actual.Should().BeEquivalentTo(customer);
    }


    //private Mock<FunctionContext> SetupFunctionContext()
    //{
    //    ServiceCollection serviceCollection = new ServiceCollection();
    //    serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
    //    ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

    //    var context = new Mock<FunctionContext>();
    //    return context.SetupProperty(c => c.InstanceServices, serviceProvider);
    //}

    //private static FunctionContext CreateContext(ObjectSerializer? serializer = null)
    //{
    //    var context = new MockFunctionContext();

    //    var services = new ServiceCollection();
    //    services.AddOptions();
    //    services.AddLogging();
    //    services.AddFunctionsWorkerCore();

    //    services.Configure<WorkerOptions>(c =>
    //    {
    //        c.Serializer = serializer;
    //    });

    //    context.InstanceServices = services.BuildServiceProvider();

    //    return context;
    //}

}
