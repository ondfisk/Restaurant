using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Restaurant.Infrastructure.RestaurantContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Restaurant"));
});
builder.Services.AddScoped<IReservationsRepository, Restaurant.Infrastructure.ReservationsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Hello World!").ConfigureAwait(false);
});

app.MapPost("/reservations", async (IReservationsRepository repository, ReservationDto reservation) =>
{
    if (string.IsNullOrWhiteSpace(reservation.Email))
        return Results.BadRequest("Email is required.");

    if (reservation.Email.Length > 50)
        return Results.BadRequest("Email is too long.");

    // validate email
    try
    {
        var res = new EmailAddressAttribute().GetValidationResult(reservation.Email, new ValidationContext(reservation));

        if (res != ValidationResult.Success)
            return Results.BadRequest("Email is invalid.");
    }
    catch (Exception)
    {
        return Results.BadRequest("Email is invalid.");
    }

    if (string.IsNullOrWhiteSpace(reservation.Name))
        return Results.BadRequest("Name is required.");

    if (reservation.Name.Length > 50)
        return Results.BadRequest("Name is too long.");

    if (reservation.Quantity <= 0)
        return Results.BadRequest("Quantity must be greater than 0.");

    if (reservation.Quantity > 20)
        return Results.BadRequest("Quantity must be at most 20.");

    await repository.Create(new Reservation(reservation.At, reservation.Email, reservation.Name, reservation.Quantity)).ConfigureAwait(false);

    return Results.NoContent();
});

app.Run();

public partial class Program { }
