

using System.Net.Http.Json;

namespace Restaurant.RestAPI.Tests;

public sealed class ReservationsTests
{
    [Fact]
    public async Task Post_Valid_Reservations()
    {
        var response = await PostReservation(new
        {
            date = "2023-03-10 19:00",
            email = "katinka@example.com",
            name = "Katinka Ingabogovinanana",
            quantity = 2
        });

        response.IsSuccessStatusCode.Should().BeTrue($"Actual status code: {response.StatusCode}.");
    }

    private static async Task<HttpResponseMessage> PostReservation(object reservation)
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        using var content = JsonContent.Create(reservation);

        return await client.PostAsync("/reservations", content);
    }
}
