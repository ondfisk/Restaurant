namespace Restaurant.RestAPI.Tests;

public sealed class ReservationsTests
{
    [Fact]
    public async Task PostValidReservations()
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();

        var response = await client.PostAsJsonAsync("/reservations", new
        {
            at = "2023-03-10 19:00",
            email = "katinka@example.com",
            name = "Katinka Ingabogovinanana",
            quantity = 2
        }).ConfigureAwait(false);

        response.IsSuccessStatusCode.Should().BeTrue($"Actual status code: {response.StatusCode}.");
    }

    [Fact]
    public async Task PostValidReservationWhenDatabaseIsEmpty()
    {
        using var app = new ReservationApplication();
        var repository = app.Services.GetRequiredService<IReservationsRepository>() as FakeReservationsRepository;
        var client = app.CreateClient();

        var dto = new ReservationDto
        {
            At = "2023-11-24 19:00",
            Email = "juliad@example.net",
            Name = "Julia Donna",
            Quantity = 5
        };
        await client.PostAsJsonAsync("/reservations", dto).ConfigureAwait(false);

        var expected = new Reservation(
            new DateTime(2023, 11, 24, 19, 0, 0),
            dto.Email,
            dto.Name,
            dto.Quantity
        );
        repository!.Should().Contain(expected);
    }
}
