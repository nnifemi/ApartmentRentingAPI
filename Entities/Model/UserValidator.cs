using FluentValidation;
using Entities.Model;
using Entities.Model.DTOs;

namespace Entities.Model;

public class UserValidator : AbstractValidator<UserDTO>
{
    public UserValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(user => user.UserType)
            .NotEmpty().WithMessage("User type is required.")
            .MaximumLength(50).WithMessage("User type cannot exceed 50 characters.");

        // Add more validation rules as needed
    }
}
