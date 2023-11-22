using Application.Users;
using Domain;
using FluentAssertions;
using Function.Users;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace Function.Tests.Integration.Users
{

    [Collection(IntegrationTestsCollection.Name)]
    public class SearchUserFunctionTests
        : FunctionUnitTestBase,
            IClassFixture<Program>,
            IAsyncLifetime
    {
        private readonly TestProgram _program;

        private readonly SearchUserFunction? _sut;

        public SearchUserFunctionTests(TestProgram program)
        {
            _program = program;
            _sut = _program.ServiceProvider.GetService<SearchUserFunction>();
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
        public async Task SearchUserFunctionTests_WhenRun_ShouldReturns(SearchUserDto searchUserDto)
        {
            var token = JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
                new JwtSecurityToken(
                    JwtTokenProvider.Issuer,
                    JwtTokenProvider.Issuer,
                    new List<Claim>
                    {
                        new(ClaimTypes.Role, "Operator"),
                        new("department", "Security")
                    },
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: JwtTokenProvider.SigningCredentials
                )
            );
            var headers = new HttpHeadersCollection();
            headers.Add("Authorization", $"Bearer {token}");

            searchUserDto.Name = "John";
            searchUserDto.Page = 1;
            searchUserDto.Size = 1;

            SetRequestBody(_request, searchUserDto);
            SetRequestHeaders(_request, headers);

            var response = await _sut!.Run(_request.Object);
            var actual = GetBodyObjectFromResponse<List<Customer>>(response);
            actual.Count().Should().Be(1);
            // actual.Email.Should().BeEquivalentTo(userDto.Email);
        }
    }
}
