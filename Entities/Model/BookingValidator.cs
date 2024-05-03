using FluentValidation;
using Entities.Model;
using Entities.Model.DTOs;

namespace ApartmentRentingAPI.Validators
{
    public class BookingValidator : AbstractValidator<BookingDTO>
    {
        public BookingValidator()
        {
            RuleFor(booking => booking.UserID)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(booking => booking.ApartmentID)
                .NotEmpty().WithMessage("Apartment ID is required.");

            RuleFor(booking => booking.BookingDate)
                .NotEmpty().WithMessage("Booking date is required.")
                .LessThan(booking => DateTime.Now.AddDays(30)).WithMessage("Booking date cannot be more than 30 days in the future.");

            RuleFor(booking => booking.CheckInDate)
                .NotEmpty().WithMessage("Check-in date is required.")
                .GreaterThan(booking => booking.BookingDate).WithMessage("Check-in date must be after booking date.");

            RuleFor(booking => booking.CheckOutDate)
                .NotEmpty().WithMessage("Check-out date is required.")
                .GreaterThan(booking => booking.CheckInDate).WithMessage("Check-out date must be after check-in date.");

            RuleFor(booking => booking.PaymentStatus)
                .NotEmpty().WithMessage("Payment status is required.");
        }
    }
}
