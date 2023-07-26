namespace Restaurant.RestAPI.Tests;

public sealed class FakeReservationsRepository : Collection<Reservation>, IReservationsRepository
{
    public Task Create(Reservation reservation)
    {
        Add(reservation);
        return Task.CompletedTask;
    }
}
