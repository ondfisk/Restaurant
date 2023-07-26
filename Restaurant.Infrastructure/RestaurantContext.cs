namespace Restaurant.Infrastructure;

public sealed class RestaurantContext : DbContext
{
    public DbSet<Reservation> Reservations { get; set; }

    public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        modelBuilder.Entity<Reservation>()
            .Property(p => p.Name)
            .HasMaxLength(50);

        modelBuilder.Entity<Reservation>()
            .Property(p => p.Email)
            .HasMaxLength(50);
    }
}
