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

[Collection(nameof(CreateObjectCollection))]
[Story(
    Title = "Auth and Search Customer",
    AsA = "As an Admin",
    IWant = "I want to search customer",
    SoThat = "So that I can get list of customers"
)]
public class SearchUserFunctionBDDTests : FunctionUnitTestBase
{
    private const string Given_RegisterUserTemplate = "Given_RegisterUser is ${0}";

    private readonly CreateObjectFixture _fixture;
    private HttpResponseMessage _response;
    private Customer _customer;

    public SearchUserFunctionBDDTests(CreateObjectFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory, AutoData]
    public void Execute(UserDto userDto)
    {
        userDto.Name = "John";
        userDto.Role = "Admin";
        userDto.Email = "test@test.com";

        SearchUserDto searchUserDto =
            new()
            {
                Name = "John",
                Page = 1,
                Size = 1
            };

        this.Given(r => r.Given_RegisterUser(userDto), Given_RegisterUserTemplate)
            .When(r => r.When_LoginUser(userDto))
            .Then(r => r.Then_SearchUserShouldReturn(searchUserDto))
            .BDDfy();
    }

    private async Task Given_RegisterUser(UserDto userDto)
    {
        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(userDto),
            Encoding.UTF8,
            "application/json"
        );
        _response = await _fixture.Client
            .PostAsync("api/RegisterUserFunction", content)
            .ConfigureAwait(false);
    }

    private async Task When_LoginUser(UserDto userDto)
    {
        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(userDto),
            Encoding.UTF8,
            "application/json"
        );
        _response = await _fixture.Client
            .PostAsync("api/LoginUserFunction", content)
            .ConfigureAwait(false);
        _customer = await _response.Content.ReadFromJsonAsync<Customer>();
    }

    private async Task Then_SearchUserShouldReturn(SearchUserDto searchUser)
    {
        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(searchUser),
            Encoding.UTF8,
            "application/json"
        );

        _fixture.Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _customer.Token);
        _response = await _fixture.Client
            .PostAsync("api/SearchUserFunction", content)
            .ConfigureAwait(false);

        var actual = await _response.Content.ReadFromJsonAsync<List<Customer>>();
        _response.IsSuccessStatusCode.Should().BeTrue();
        actual.Count().Should().Be(1);
    }
}
