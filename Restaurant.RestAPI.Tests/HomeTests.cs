namespace Restaurant.RestAPI.Tests;

public sealed class HomeTests
{
    [Fact]
    public async Task Home_Returns_JSON()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        using var request = new HttpRequestMessage(HttpMethod.Get, "/");
        request.Headers.Accept.ParseAdd("application/json");
        var response = await client.SendAsync(request);

        response.IsSuccessStatusCode.Should().BeTrue($"Actual status code: {response.StatusCode}.");
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }
}