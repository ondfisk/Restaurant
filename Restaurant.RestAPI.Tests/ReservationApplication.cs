using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Restaurant.RestAPI.Tests;

public sealed class ReservationApplication : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IReservationsRepository>();
            services.AddSingleton<IReservationsRepository, FakeReservationsRepository>();
        });

        builder.UseEnvironment("Development");
    }
}
