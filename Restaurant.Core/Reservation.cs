namespace Restaurant.Core;

public sealed record class Reservation(DateTime At, string Email, string Name, int Quantity);
