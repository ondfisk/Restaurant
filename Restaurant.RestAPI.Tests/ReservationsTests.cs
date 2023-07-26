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
            at = "2023-03-10T19:00",
            email = "katinka@example.com",
            name = "Katinka Ingabogovinanana",
            quantity = 2
        }).ConfigureAwait(false);

        response.IsSuccessStatusCode.Should().BeTrue($"Actual status code: {response.StatusCode}.");
    }

    [Theory]
    [InlineData(null, "juliad@example.net", "Julia Donna", 5)]
    [InlineData("2023-11-24T19:00", null, "Julia Donna", 5)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", null, 5)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Donna", null)]
    [InlineData("", "juliad@example.net", "Julia Donna", 5)]
    [InlineData("2023-11-24T19:00", "", "Julia Donna", 5)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "", 5)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Donna", "")]
    [InlineData(" ", "juliad@example.net", "Julia Donna", 5)]
    [InlineData("2023-11-24T19:00", " ", "Julia Donna", 5)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", " ", 5)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Donna", " ")]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Donna", 0)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Donna", -1)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Donna", 21)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Donna", int.MinValue)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Donna", int.MaxValue)]
    [InlineData("<bad date>", "juliad@example.net", "Julia Donna", 5)]
    [InlineData("2023-11-24T19:00", "<bad email>", "Julia Donna", 5)]
    [InlineData("2023-11-24T19:00", "juliad@example-example.example-example.example-example.net", "Julia Donna", 5)]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Example Example Example Example Example Example Example Donna", 5)]
    public async Task PostInvalidReservations(object? at, object? email, object? name, object? quantity)
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();

        var response = await client.PostAsJsonAsync("/reservations", new { at, email, name, quantity }).ConfigureAwait(false);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostNullReservation()
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();

        var response = await client.PostAsJsonAsync("/reservations", default(string)).ConfigureAwait(false);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostArrayReservation()
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();

        var response = await client.PostAsJsonAsync("/reservations", Array.Empty<int>()).ConfigureAwait(false);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("2023-11-24T19:00", "juliad@example.net", "Julia Donna", 5)]
    [InlineData("2024-02-13T18:15", "x@example.com", "Xenia Ng", 9)]
    public async Task PostValidReservationWhenDatabaseIsEmpty(string at, string email, string name, int quantity)
    {
        using var app = new ReservationApplication();
        var repository = app.Services.GetRequiredService<IReservationsRepository>() as FakeReservationsRepository;
        var client = app.CreateClient();

        var dto = new ReservationDto(DateTime.Parse(at, CultureInfo.InvariantCulture), email, name, quantity);
        await client.PostAsJsonAsync("/reservations", dto).ConfigureAwait(false);

        var expected = new Reservation(dto.At, dto.Email, dto.Name, dto.Quantity);
        repository!.Should().Contain(expected);
    }
}
