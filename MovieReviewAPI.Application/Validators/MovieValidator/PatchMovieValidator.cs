using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Application.Commands.Movie;
using MovieReviewApi.Application.Interfaces;

namespace MovieReviewApi.Application.Validators.MovieValidator
{
    public class PatchMovieValidator : AbstractValidator<PatchMovieCommand>
    {
        private readonly IApplicationDbContext _context;

        public PatchMovieValidator(IApplicationDbContext context)
        {
            _context = context;

            // Ensure dto itself is not null
            RuleFor(x => x.dto)
                .NotNull()
                .WithMessage("Movie data (dto) is required");

            // Title
            RuleFor(x => x.dto!.Title)
                .Length(5, 300)
                .WithMessage("Title must be 5-300 characters")
                .When(x => x.dto?.Title != null)
                .MustAsync(async (command, title, ct) =>
                {
                    if (string.IsNullOrWhiteSpace(title)) return true;

                    return !await _context.Movies
                        .AnyAsync(m => !string.IsNullOrEmpty(m.Title) &&
                                       m.Title.ToLower().Trim() == title.ToLower().Trim(), ct);
                })
                .WithMessage((command, title) => $"Movie with title '{title}' already exists");

            // Description
            RuleFor(x => x.dto!.Description)
                .MinimumLength(10)
                .WithMessage("Description must be at least 10 characters")
                .MaximumLength(1000)
                .WithMessage("Description cannot exceed 1000 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.dto?.Description));

            // ReleaseDate
            RuleFor(x => x.dto!.ReleaseDate)
                .LessThan(DateTime.UtcNow)
                .WithMessage("Release date must be in the past")
                .When(x => x.dto?.ReleaseDate.HasValue == true);

            // DurationMinutes
            RuleFor(x => x.dto!.DurationMinutes)
                .InclusiveBetween(1, 600)
                .WithMessage("Duration must be between 1 and 600 minutes")
                .When(x => x.dto?.DurationMinutes.HasValue == true);

            // Rating
            RuleFor(x => x.dto!.Rating)
                .InclusiveBetween(0, 10)
                .WithMessage("Rating must be between 0 and 10")
                .When(x => x.dto?.Rating.HasValue == true);

            // ActorIds
            RuleFor(x => x.dto!.ActorIds)
                .Must(HaveValidGuids)
                .WithMessage("All actor IDs must be valid GUIDs")
                .When(x => x.dto?.ActorIds != null && x.dto.ActorIds.Any());
        }

        private static bool HaveValidGuids(List<Guid>? ids)
        {
            return ids?.All(id => id != Guid.Empty) ?? true;
        }
    }
}
