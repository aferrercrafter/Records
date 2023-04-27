using BallastLane.Domain.Entities;
using FluentValidation;

namespace BallastLane.Domain.Validation;

public class RecordValidator : AbstractValidator<Record>
{
    public RecordValidator()
    {
        RuleFor(record => record.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(record => record.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
