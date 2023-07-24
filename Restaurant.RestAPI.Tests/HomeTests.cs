namespace Restaurant.RestAPI.Tests;

public class HomeTests
{
    [Fact]
    public async Task HomeIsOk()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        
        var response = await client.GetAsync("/");

        response.IsSuccessStatusCode.Should().BeTrue($"Actual status code: {response.StatusCode}.");
    }
}