namespace Restaurant.RestAPI.Tests;

public sealed class HomeTests
{
    [Fact]
    public async Task HomeReturnsJson()
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();

        using var request = new HttpRequestMessage(HttpMethod.Get, "/");
        request.Headers.Accept.ParseAdd("application/json");
        var response = await client.SendAsync(request).ConfigureAwait(false);

        response.IsSuccessStatusCode.Should().BeTrue($"Actual status code: {response.StatusCode}.");
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }
}