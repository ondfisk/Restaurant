namespace Restaurant.Infrastructure;

public sealed class ReservationsRepository : IReservationsRepository
{
    private readonly RestaurantContext _context;

    public ReservationsRepository(RestaurantContext context)
    {
        _context = context;
    }

    public async Task Create(Core.Reservation reservation)
    {
        if (reservation is null)
        {
            throw new ArgumentNullException(nameof(reservation));
        }

        var entity = new Reservation
        {
            At = reservation.At,
            Email = reservation.Email,
            Name = reservation.Name,
            Quantity = reservation.Quantity
        };

        _context.Reservations.Add(entity);

        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
