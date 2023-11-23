using Application.Users;
using Domain;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text;

namespace Function.Tests.Integration.Users
{
    public class SearchUserFunctionTests3 : FunctionUnitTestBase,IClassFixture<CreateObjectFixture>
    {
        private readonly CreateObjectFixture _fixture;

        public SearchUserFunctionTests3(CreateObjectFixture fixture)
        {
            this._fixture = fixture;
        }

        [Theory, CustomAutoData]
        public async Task ValidBodyReturnCompleteNa(UserDto userDto)
        {
            // Arrange
            userDto.Email = "test@test.com";
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");

            // Act
            var _response = await _fixture.Client.PostAsync("api/LoginUserFunction", content);
            var actual = await _response.Content.ReadFromJsonAsync<Customer>();

            // Assert
            _response.IsSuccessStatusCode.Should().BeTrue();
            actual.Email.Should().Be(userDto.Email);
        }
    }
}
