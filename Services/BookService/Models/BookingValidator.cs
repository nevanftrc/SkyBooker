using BookService.Models;
using FluentValidation;

namespace BookService.Validation;

public class BookingValidator : AbstractValidator<Booking>
{
    public BookingValidator()
    {
        RuleFor(x => x.PassengerFirstname)
            .NotEmpty().WithMessage("Vorname darf nicht leer sein.");

        RuleFor(x => x.PassengerLastname)
            .NotEmpty().WithMessage("Nachname darf nicht leer sein.");

        RuleFor(x => x.FlightId)
            .NotEmpty().WithMessage("FlightId ist erforderlich.");

        RuleFor(x => x.TicketCount)
            .GreaterThan(0).WithMessage("Es muss mindestens ein Ticket gebucht werden.");
    }
}
