using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Validators.ActorValidator;

public class PatchActorValidator : AbstractValidator<PatchActorCommand>
{
    private readonly IApplicationDbContext _context;

    public PatchActorValidator(IApplicationDbContext context)
    {
        _context = context;

        // Ensure dto is not null
        RuleFor(x => x.dto)
            .NotNull()
            .WithMessage("Actor data (dto) is required");

        // Name
        RuleFor(x => x.dto!.Name)
            .MinimumLength(2).WithMessage("Name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Name can't exceed 100 characters")
            .When(x => x.dto?.Name != null)
            .MustAsync(async (command, name, ct) =>
            {
                if (string.IsNullOrWhiteSpace(name)) return true;

                return !await _context.Actors
                    .AnyAsync(a => !string.IsNullOrEmpty(a.Name) &&
                                   a.Name.ToLower().Trim() == name.ToLower().Trim(), ct);
            })
            .WithMessage((command, name) => $"Actor with the name '{name}' already exists");

        // Bio
        RuleFor(x => x.dto!.Bio)
            .MinimumLength(10).WithMessage("Bio must be at least 10 characters")
            .MaximumLength(500).WithMessage("Bio can't exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.dto?.Bio));

        // DateOfBirth
        RuleFor(x => x.dto!.DateOfBirth)
            .Must(BeInThePast)
            .WithMessage("Date of birth must be in the past")
            .When(x => x.dto?.DateOfBirth.HasValue == true);
    }

    private static bool BeInThePast(DateTime? dateOfBirth)
    {
        return !dateOfBirth.HasValue || dateOfBirth.Value < DateTime.UtcNow;
    }
}
