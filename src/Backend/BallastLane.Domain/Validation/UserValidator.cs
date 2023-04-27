using BallastLane.Domain.Entities;
using FluentValidation;

namespace BallastLane.Domain.Validation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Username).NotEmpty().WithMessage("Username is required.")
                                 .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(u => u.Email).NotEmpty().WithMessage("Email address is required.")
                             .EmailAddress().WithMessage("Invalid email address.")
                             .MaximumLength(100).WithMessage("Email address cannot exceed 100 characters.");
    }
}
