using FluentValidation;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;


namespace MovieReviewApi.Application.Validators.ActorValidator;
public class UpdateActorValidator : AbstractValidator<UpdateActorCommand>
{
    public UpdateActorValidator()
    {
        RuleFor(x => x.dto.Name)
            .NotEmpty().WithMessage("Actor name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Name can't exceed 100 characters");

        RuleFor(x => x.dto.DateOfBirth)
            .Must(BeInThePast)
            .WithMessage("Date of birth must be in the past")
            .When(x => x.dto.DateOfBirth.HasValue);

        RuleFor(x => x.dto.Bio)
            .MaximumLength(500).WithMessage("Bio can't exceed 500 characters")
            .MinimumLength(10).WithMessage("Bio must be at least 10 characters long")
            .When(x => !string.IsNullOrEmpty(x.dto.Bio));

        RuleFor(x => x.dto.MovieIds)
            .Must(HaveValidGuids)
            .WithMessage("All movie IDs must be valid GUIDs")
            .When(x => x.dto.MovieIds != null && x.dto.MovieIds.Any());
    }

    private static bool BeInThePast(DateTime? dateOfBirth)
    {
        return !dateOfBirth.HasValue || dateOfBirth.Value < DateTime.UtcNow;
    }

    private static bool HaveValidGuids(List<Guid>? movieIds)
    {
        return movieIds?.All(id => id != Guid.Empty) ?? true;
    }
}