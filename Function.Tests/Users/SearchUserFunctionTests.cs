using Application.Users;
using Application.Users.Queries;
using Domain;
using FluentAssertions;
using Function.Users;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Function.Tests.Users
{
    public class SearchUserFunctionTests : FunctionUnitTestBase
    {
        private readonly Mock<ILoggerFactory> _logger;
        private readonly Mock<IMediator> _mediator;
        private readonly SearchUserFunction _sut;

        public SearchUserFunctionTests()
        {
            var loggerMock = new Mock<ILogger<RegisterUserFunction>>();
            _logger = new Mock<ILoggerFactory>();
            _logger.Setup(r => r.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

            _mediator = new Mock<IMediator>();

            _sut = new SearchUserFunction(_mediator.Object, _logger.Object);
        }

        [Theory, CustomAutoData]
        public async Task LoginUserFunctionTests_WhenRun_ShouldReturns(
            List<Customer> customers,
            UserDto userDto
        )
        {
            userDto.Email = "test@test.com";
            SetRequestBody(_request, userDto);

            _mediator
                .Setup(r => r.Send(It.IsAny<SearchUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customers);

            var response = await _sut.Run(_request.Object);

            var actual = GetBodyObjectFromResponse<List<Customer>>(response);

            actual.Should().BeEquivalentTo(customers);
        }
    }
}
