using FluentValidation;
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
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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

app.MapPost("/reservations", async (IReservationsRepository repository, ReservationDtoValidator validator, ReservationDto reservation) =>
{
    var validationResult = validator.Validate(reservation);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.ToDictionary());
    }

    await repository.Create(new Reservation(reservation.At, reservation.Email, reservation.Name, reservation.Quantity)).ConfigureAwait(false);

    return Results.NoContent();
});

app.Run();

public partial class Program { }
