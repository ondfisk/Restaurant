namespace Restaurant.Infrastructure;

public sealed class RestaurantContext : DbContext
{
    public DbSet<ReservationEntity> Reservations { get; set; }

    public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        modelBuilder.Entity<ReservationEntity>()
            .Property(p => p.Name)
            .HasMaxLength(50);

        modelBuilder.Entity<ReservationEntity>()
            .Property(p => p.Email)
            .HasMaxLength(50);
    }
}

// Add migration using:
// dotnet ef migrations add <name> --project .\Restaurant.Infrastructure\ --startup-project .\Restaurant.RestAPI\
// Update using:
// dotnet ef migrations remove --project .\Restaurant.Infrastructure\ --startup-project .\Restaurant.RestAPI\
