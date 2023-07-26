namespace Restaurant.RestAPI.Tests;

public sealed class ReservationsTests
{
    [Fact]
    public async Task PostValidReservation()
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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("<bad date>")]
    [InlineData("2023-02-34T25:00")]
    public async Task PostReservationWithInvalidDate(string at)
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();

        var response = await client.PostAsJsonAsync("/reservations", new { at, email = "juliad@example.net", name = "Julia Donna", quantity = 5 }).ConfigureAwait(false);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("<bad email>")]
    [InlineData("too_long@example-example.example-example.example-example.net")]
    public async Task PostReservationWithInvalidEmail(string? email)
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();

        var response = await client.PostAsJsonAsync("/reservations", new { at = "2023-11-24T19:00", email, name = "Julia Donna", quantity = 5 }).ConfigureAwait(false);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Julia Too Long Too Long Too Long Too Long Too Long Too Long Too Long Too Long Donna")]
    public async Task PostReservationWithInvalidName(string? name)
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();

        var response = await client.PostAsJsonAsync("/reservations", new { at = "2023-11-24T19:00", email = "juliad@example.net", name, quantity = 5 }).ConfigureAwait(false);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("4,2")]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(0.5)]
    public async Task PostReservationWithInvalidQuantity(object? quantity)
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();

        var response = await client.PostAsJsonAsync("/reservations", new { at = "2023-11-24T19:00", email = "juliad@example.net", name = "Julia Donna", quantity }).ConfigureAwait(false);

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

    [Fact]
    public async Task PostInvalidJsonReservation()
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();
        using var content = new StringContent("{]", Encoding.UTF8, "application/json");

        var response = await client.PostAsync(new Uri("/reservations", UriKind.Relative), content).ConfigureAwait(false);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostNonJsonReservation()
    {
        using var app = new ReservationApplication();
        var client = app.CreateClient();
        using var content = new StringContent("24-11-2023, juliad@example.net, Julia Donna, 5", Encoding.UTF8, "text/plain");

        var response = await client.PostAsync(new Uri("/reservations", UriKind.Relative), content).ConfigureAwait(false);

        response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
    }
}
