using System.Diagnostics;
using System.Net.Http;
using Azure.Core;
using System.Text;
using Polly;
using Polly.Retry;
using TestStack.BDDfy;
using System.Text.Json;
using Application.Users;

namespace Function.Tests.Integration.Users;

[Collection(nameof(TestCollection))]
public class SearchUserFunctionBDDTests
{
    private readonly TestFixture _fixture;
    private HttpResponseMessage _response;

    public SearchUserFunctionBDDTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory, CustomAutoData]
    public async Task WhenfunctionIsInvoked(UserDto userDto)
    {
        userDto.Email = "test@test.com";

        var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");

        var result = await _fixture.Client.PostAsync("api/LoginUserFunction", content);
        Assert.True(result.IsSuccessStatusCode);
    }

    //[Fact]
    //public void SearchUserFunctionBDD_Test()
    //{
    //    this.BDDfy();
    //}

    //private async Task When_SearchUserFunctionBDD_Is_Invoked()
    //{
    //    _response = await _fixture.HttpClient.PostAsJsonAsync("api/HelloWorld?name=James+Bond");
    //}

    //private async Task Then_The_Response_Should_Return()
    //{
    //    Assert.EndsWith("James Bond", await _response.Content.ReadAsStringAsync());
    //}
}
