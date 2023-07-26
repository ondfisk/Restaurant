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
    await repository.Create(new Reservation(DateTime.Parse(reservation.At!, CultureInfo.InvariantCulture), reservation.Email!, reservation.Name!, reservation.Quantity)).ConfigureAwait(false);

    return Results.NoContent();
});

app.Run();

public partial class Program { }
