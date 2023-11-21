using Application.Users;
using Function.Users;
using Microsoft.Extensions.DependencyInjection;
using Domain;
using FluentAssertions;

namespace Function.Tests.Integration.Users
{
    [Collection(IntegrationTestsCollection.Name)]
    public class RegisterUserFunctionTests
        : FunctionUnitTestBase,
            IClassFixture<Program>,
            IAsyncLifetime
    {
        private readonly TestProgram _program;

        private readonly RegisterUserFunction? _sut;

        public RegisterUserFunctionTests(TestProgram program)
        {
            _program = program;

            _sut = _program.ServiceProvider.GetService<RegisterUserFunction>();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        [Theory, CustomAutoData]
        public async Task RegisterUserFunctionTests_WhenRun_ShouldReturns(UserDto userDto)
        {
            userDto.Email = "test@test.com";
            SetRequestBody(_request, userDto);

            var response = await _sut!.Run(_request.Object);
            var actual = GetBodyObjectFromResponse<Customer>(response);
            actual.Email.Should().BeEquivalentTo(userDto.Email);
        }
    }
}
