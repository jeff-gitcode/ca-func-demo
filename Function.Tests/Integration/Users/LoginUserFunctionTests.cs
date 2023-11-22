using Application.Users;
using Domain;
using FluentAssertions;
using Function.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Function.Tests.Integration.Users
{
    [Collection(IntegrationTestsCollection.Name)]
    public class LoginUserFunctionTests
       : FunctionUnitTestBase,
           IClassFixture<Program>,
           IAsyncLifetime
    {
        private readonly TestProgram _program;

        private readonly LoginUserFunction? _sut;

        public LoginUserFunctionTests(TestProgram program)
        {
            _program = program;
            _sut = _program.ServiceProvider.GetService<LoginUserFunction>();
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
        public async Task LoginUserFunctionTests_WhenRun_ShouldReturns(UserDto userDto)
        {
            userDto.Email = "test@test.com";
            SetRequestBody(_request, userDto);

            var response = await _sut!.Run(_request.Object);
            var actual = GetBodyObjectFromResponse<Customer>(response);
            actual.Email.Should().BeEquivalentTo(userDto.Email);
            actual.Token.Should().NotBeEmpty();
        }
    }
}
