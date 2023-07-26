namespace Restaurant.Infrastructure;

public sealed class ReservationEntity
{
    public int Id { get; set; }
    public required DateTime At { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required int Quantity { get; set; }
}