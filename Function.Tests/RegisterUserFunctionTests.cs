using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Function.Users;
using MediatR;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Function.Tests;


public class RegisterUserFunctionTests
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
    public async Task RegisterUserFunctionTests_When_ShouldReturns([Frozen] Mock<HttpRequestData> httpRequestData, [Frozen] Mock<HttpResponseData> httpResponseData)
    {
        httpResponseData.SetupGet(x => x.StatusCode)
               .Returns(HttpStatusCode.OK);

        httpRequestData.Setup(x => x.CreateResponse())
                .Returns(httpResponseData.Object);

        var actual = await _sut.Run(httpRequestData.Object);
    }
}
