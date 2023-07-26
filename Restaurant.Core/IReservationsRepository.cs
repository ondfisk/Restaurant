namespace Restaurant.Core;

public interface IReservationsRepository
{
    Task Create(Reservation reservation);
}
