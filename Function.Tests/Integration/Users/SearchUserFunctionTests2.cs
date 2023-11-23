using System.Diagnostics;
using System.Net.Http;
using Azure.Core;
using System.Text;
using Polly;
using Polly.Retry;
using TestStack.BDDfy;
using System.Text.Json;
using Application.Users;
using FluentAssertions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Domain;
using System.Net.Http.Json;
using AutoFixture;
using AutoFixture.Xunit2;

namespace Function.Tests.Integration.Users;

[Collection(nameof(TestCollection))]
public class SearchUserFunctionTests2 : FunctionUnitTestBase
{
    private readonly TestFixture _fixture;
    private HttpResponseMessage _response;

    public SearchUserFunctionTests2(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory, CustomAutoData]
    public async Task WhenfunctionIsInvoked(UserDto userDto)
    {
        userDto.Email = "test@test.com";

        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");

        _response = await _fixture.Client.PostAsync("api/LoginUserFunction", content);
        var actual = await _response.Content.ReadFromJsonAsync<Customer>();

        _response.IsSuccessStatusCode.Should().BeTrue();
        actual.Email.Should().Be(userDto.Email);
    }
}
