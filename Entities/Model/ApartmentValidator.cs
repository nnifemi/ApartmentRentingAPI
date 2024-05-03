using FluentValidation;
using Entities.Model;
using Entities.Model.DTOs;

namespace Entities.Model
{
    public class ApartmentValidator : AbstractValidator<ApartmentDTO>
    {
        public ApartmentValidator()
        {
            RuleFor(apartment => apartment.LandlordID)
                .NotEmpty().WithMessage("Landlord ID is required.");

            RuleFor(apartment => apartment.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(100).WithMessage("Location cannot exceed 100 characters.");

            RuleFor(apartment => apartment.FlatNumber)
                .NotEmpty().WithMessage("Flat number is required.")
                .MaximumLength(50).WithMessage("Flat number cannot exceed 50 characters.");

            RuleFor(apartment => apartment.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(apartment => apartment.Amenities)
                .NotEmpty().WithMessage("Amenities are required.");

            RuleFor(apartment => apartment.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(apartment => apartment.Availability)
                .NotNull().WithMessage("Availability is required.");
        }
    }
}
