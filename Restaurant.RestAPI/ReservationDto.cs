namespace Restaurant.RestAPI;

public sealed record class ReservationDto(DateTime At, string Email, string Name, int Quantity);

public class ReservationDtoValidator : AbstractValidator<ReservationDto>
{
    public ReservationDtoValidator()
    {
        RuleFor(x => x.At).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0).LessThanOrEqualTo(20);
    }
}
