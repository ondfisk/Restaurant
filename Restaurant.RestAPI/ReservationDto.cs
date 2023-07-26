namespace Restaurant.RestAPI;

public sealed class ReservationDto
{
    public DateTime At { get; set; }
    public required string Email { get; init; }
    public required string Name { get; init; }
    public int Quantity { get; set; }
}
